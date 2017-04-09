
(function($) { // encapsulate jQuery

    var charts = new Array();

    function createSummaryChart(tsv) {


      

        // Build the chart
        var chart = new Highcharts.Chart({
            chart: {
                renderTo: 'piegraph',
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false
            },
            credits: {
                enabled: false
            },
            title: {
                text: ''
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage}%</b>',
                percentageDecimals: 2
            },
            plotOptions: {
                pie: {
                    dataLabels: {
                        enabled: true,
                        color: '#000000',
                        connectorColor: '#000000',
                        formatter: function () {
                            return '<b>' + this.point.name + '</b>: ' + this.percentage + ' %';
                        }
                    }
                }
            },
            series: [{
                type: 'pie',
                name: 'Application Usage',
                data: tsv.Summary
            }]
        });
    }
    
    function createHourlyChart(tsv, i) {

        var obj = tsv.HourlyUsage[i];
        if (obj.length === 0) {

            return;
        }

        var hourDate = new Date();
        hourDate.setHours(i);
        
        var newid = 'hour' + i;
        
        // Table view
        var table = document.getElementById('hourly-table');

        var rowCount = table.rows.length;
        var row = table.insertRow(rowCount);

        var formattedDate;
        if (i == 0) {
            formattedDate = "Midnight";
        }
        else if (i == 12) {
            formattedDate = "Noon";
        }
        else if (i < 12) {
            formattedDate = i + "&nbsp;am";
        } else {
            formattedDate = (i % 12) + "&nbsp;pm";
        }

        var cell1 = row.insertCell(0);
        cell1.setAttribute('style', 'width:12%; vertical-align:middle');

        var h2 = document.createElement('h2');

        var h2span = document.createElement('h2span');
        
        h2span.innerHTML = formattedDate;
        h2.appendChild(h2span);
        cell1.appendChild(h2);

        var cell2 = row.insertCell(1);
        cell2.setAttribute('style', "width:88%;");
        var hour = document.createElement("div");
        
        

        hour.id = newid;
        cell2.appendChild(hour);
        
        charts[i]= new Highcharts.Chart({
            chart: {
                renderTo: newid,
                type: 'bar',
                height: 100,
                spacingTop: 1,
                spacingRight: 1,
                spacingBottom: 1,
                margin: 1,
                tickLength: 1
                
            },
            credits: {
                enabled: false
            },
            title: {
                text: ''
            },
            xAxis: {
                categories: ['']
            },
            yAxis: {
                min: 0,
                reversed: true,
                tickInterval : 600,
                title: {
                    text: 'Minutes'
                }
                , max: 3600

                ,
                labels: {
                    formatter: function () {
                        return 60 - (this.value / 60);
                    }
                },
                minorGridLineWidth: 0,
                GridLineWidth: 0
            },
            legend: {
                backgroundColor: '#FFFFFF',
                reversed: true,
                enabled: false
            },
            tooltip: {
                formatter: function () {
                    return '' +
                        this.series.name + ': ' + this.y + '';
                }
            },
            plotOptions: {
                series: {
                    stacking: 'normal',
                    groupPadding: 0
                }
            },
            series: obj
        });
        
        

    }

    function displayGraphs(utcYear, utcMonth, utcDay) {

        document.getElementById("piegraph").innerHTML = '';
        document.getElementById("hourlycharts").innerHTML = '';
        document.getElementById("hourly-table").innerHTML = '';

        var displayDate = new Date();
        displayDate.setFullYear(utcYear);
        displayDate.setMonth(utcMonth - 1);
        displayDate.setDate(utcDay);
        displayDate.setHours(0, 0, 0, 0);
        
        var date = displayDate.toLocaleDateString();
        document.getElementById('dailyTitle').innerHTML = date;

        var msSinceEpoch = displayDate.getTime();
        var offset = new Date().getTimezoneOffset();
        
        jQuery.get('/api/data/?msSinceEpoch=' + msSinceEpoch + '&minutesOffset=' + offset,
            null, function (tsv) {
           if (tsv != null) {

               if (tsv.Summary.length == 0) {

                   document.getElementById("no-data").innerHTML = "<i class=\"icon-user\"></i><strong>Hold on!</strong> There is no data available for this day.";
                   document.getElementById("no-data").style.display = "inline-block";
               } else {
                   
                   document.getElementById("no-data").style.display = "none";
                   createSummaryChart(tsv);

                   for (var i = 0; i < tsv.HourlyUsage.length; i++) {

                       createHourlyChart(tsv, i);
                   }
               }
           }
       });

    }

    $(document).ready(function() {

        var datePicker = document.getElementById('datepicker-submit');
        if (datePicker == null) {
            var b = new Date();

            var utcYear = b.getFullYear();
            var utcMonth = b.getMonth() + 1;
            var utcDay = b.getDate(); // horrible naming

            displayGraphs(utcYear, utcMonth, utcDay);
        }
        else {
            datePicker.onclick = function () {
                var date = document.getElementById('datepicker');
                var values = date.value.split('-');

                utcYear = values[0];
                utcMonth = values[1];
                utcDay = values[2];

                displayGraphs(utcYear, utcMonth, utcDay);
            };
        }
    });


})(jQuery);