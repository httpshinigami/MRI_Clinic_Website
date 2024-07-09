$(document).ready(function () {
    // For date of birth
    $('.datepicker').datepicker({
        format: 'yyyy-mm-dd',
        startDate: '2023-08-20',
        autoclose: true
    }).datepicker("setDate", new Date(2023, 7, 20));
});