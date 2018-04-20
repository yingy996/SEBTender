//using intlTelInput jQuery plugin for validating international phone number
var telInput = $("#phone"),
    validMsg = $("#validMsg"),
    errorMsg = $("#errorMsg");

//initialise plugin
telInput.intlTelInput({
    utilsScript: "js/utils.js", //enabling validation/formatting
    preferredCountries: ['my', 'sg']
});

//hide the valid/error messages 
var reset = function() {
    telInput.removeClass("error");
    errorMsg.addClass("hide");
    validMsg.addClass("hide");
};


telInput.blur(function() {
    reset();
    if ($.trim(telInput.val())) {
        if (telInput.intlTelInput("isValidNumber")) {
            validMsg.removeClass("hide");
        } else {
            telInput.addClass("error");
            errorMsg.removeClass("hide");
        }
    }
});

//reset when user keyin something new in the phone number field
telInput.on("keyup change", reset);
/*$(document).ready(function() {
    $('#registerForm').find('[name="phoneNum"]')
        .intlTelInput({
            utilsScript: 'utils.js',
            autoPlaceholder: true,
            preferredCountries: ['my', 'sg']
        });
    
    $('#registerForm').formValidation({
        framework: 'bootstrap',
        icon: {
            vallid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            phoneNum: {
                validators: {
                    callback: {
                        message: '*Phone number is not valid',
                        callback: function(value, validator, $field){
                            return value === '' || $field.intlTelInput('isValidNumber');
                        }
                    }
                }
            }
        }
    })
    
    //Revalidation of phone number after user changes country field
    .on('click', '.country-list', function() {
        $('#registerForm').formValidation('revalidateField', 'phoneNum');
    });
});*/