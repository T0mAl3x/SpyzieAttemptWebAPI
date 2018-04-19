/* Set the width of the side navigation to 250px */
function openNav() {
    document.getElementById("mySidenav").style.width = "250px";
}

/* Set the width of the side navigation to 0 */
function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
}
$(document).ready(function () {
    $('#smartphonesTable tr').click(function () {
        var counter = 0;
        $(this).find('td').each(function (column, td) {
            if (counter === 2) {
                document.getElementById("smartphonesInput").value = td.innerText;
                alert("You selected the smartphone with the following IMEI: " + td.innerText);
            }
            counter = counter + 1;
        });

        document.getElementById("smartphonesForm").submit();
    });
});
