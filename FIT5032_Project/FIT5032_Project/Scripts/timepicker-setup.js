$(document).ready(function () {
    $('.timepicker').timepicker({
        showMeridian: false, // Hide AM/PM
        minuteStep: 60, // Set the minute step to 60 (1 hour)
        defaultTime: '00:00', // Set the default time to midnight (00:00)
        template: 'modal', // Use a modal-style time picker
        appendWidgetTo: 'body' // Append the widget to the body for proper positioning
    });
});
