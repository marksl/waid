
(function($) { // encapsulate jQuery

    function createHourlyChart(tsv) {

        // Table view
        var table = document.getElementById('hourly-table');


        var atleastOne = false;
        for (var i = 0; i < tsv.length; i++) {

            var curr = tsv[i];

            var rowCount = table.rows.length;
            var row = table.insertRow(rowCount);
            row.setAttribute('class', 'success');

            var cell0 = row.insertCell(0);
            cell0.innerHTML = curr.startDate;

            var cell1 = row.insertCell(1);
            cell1.innerHTML = curr.startDateString;

            var cell2 = row.insertCell(2);
            cell2.innerHTML = 'App';
            
            var cell3 = row.insertCell(3);
            cell3.innerHTML = 'Seconds';
            
            for (var j = 0; j < tsv[i].apps.length; j++) {

                rowCount = table.rows.length;
                var nextRow = table.insertRow(rowCount);
                

                nextRow.insertCell(0);
                nextRow.insertCell(1);
                cell2 = nextRow.insertCell(2);
                cell2.innerHTML = tsv[i].apps[j];
                cell3 = nextRow.insertCell(3);
                cell3.innerHTML = tsv[i].appTimes[j];

                atleastOne = true;
            }
        }
        
        if (atleastOne == false) {
            document.getElementById("no-data").innerHTML = "<i class=\"icon-user\"></i><strong>Hold on!</strong> There is no data available for this day.";
            document.getElementById("no-data").style.display = "inline-block";
        } else {
            document.getElementById("no-data").style.display = "none";
        }

    }

    function displayTables(utcYear, utcMonth, utcDay) {

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
        
        jQuery.get('/api/table/?msSinceEpoch=' + msSinceEpoch + '&minutesOffset=' + offset,
            null, function (tsv) {
                if (tsv != null) {
                    createHourlyChart(tsv);
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

            displayTables(utcYear, utcMonth, utcDay);
        } else {
            
            datePicker.onclick = function () {

                var date = document.getElementById('datepicker');

                var values = date.value.split('-');

                utcYear = values[0];
                utcMonth = values[1];
                utcDay = values[2];

                displayTables(utcYear, utcMonth, utcDay);

            };
        }

    });


})(jQuery);