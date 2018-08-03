// Write your JavaScript code.
function ConfirmPrice() {
    if (parseFloat(document.getElementById("txtPrice").value) > 999) {
        alert("Entered Amount is > 999. Please confirm.");
    }
}

$(document).ready(function () {
    $("#productInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#productTable tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});
