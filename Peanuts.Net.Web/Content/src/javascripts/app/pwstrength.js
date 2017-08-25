"use strict";


/*
 * This function adds data object to an input with pw-strength-functionality. This is only for mocking.
 */

(function($){
    const options = {
        common: {
            minChar: 8
        },
        i18n: {
            t: function(key) {
                switch (key) {
                    case 'wordLength': {
                        return 'Das Passwort ist zu kurz';
                    }
                    case 'wordNotEmail': {
                        return 'Das Passwort darf die E-Mail Adresse nicht enthalten';
                    }
                    case 'wordSimilarToUsername': {
                        return 'Das Passwort darf den Benutzernamen nicht enthalten';
                    }
                    case 'wordTwoCharacterClasses': {
                        return 'Bitte Buchstaben und Ziffern verwenden';
                    }
                    case 'wordRepetitions': {
                        return 'Zu viele Wiederholungen';
                    }
                    case 'wordSequences': {
                        return 'Das Passwort enthält Buchstabensequenzen';
                    }
                    case 'errorList': {
                        return 'Fehler';
                    }
                    case 'veryWeak': {
                        return 'Sehr Schwach';
                    }
                    case 'weak': {
                        return 'Schwach';
                    }
                    case 'normal': {
                        return 'Normal';
                    }
                    case 'medium': {
                        return 'Mittel';
                    }
                    case 'strong': {
                        return 'Stark';
                    }
                    case 'veryStrong': {
                        return 'Sehr stark';
                    }
                    default: {
                        return '';
                    }
                }
            }
        }
    };

    $("#sg-js-pw-strength").data("options", options);

}(jQuery));


const PwStrength = (function ($) {

    /**
     * Prepare Inputs
     */
    const decoratorSelector = '.js-pw-strength-decorator';
    const $decorators = $(decoratorSelector);
    const inputSelectorClass = 'js-pw-strength';
    const inputSelector = '.' + inputSelectorClass;

    $decorators.each(function(index, decorator) {
        var $decorator = $(decorator);
        var decoratedInputSelector = $decorator.attr('for');
        var $decoratedInput = $(decoratedInputSelector);

        $decoratedInput.addClass(inputSelectorClass);
        var decoratorOptions = $decorator.data('options');
        $decoratedInput.data('options', decoratorOptions);
    });



    
    const $inputs = $(inputSelector);
    const config = $inputs.data('options');

    if ($inputs.length)
        loadScripts(config);

    function loadScripts(config) {
        let script = document.createElement('script');
        script.onload = initPasswordStrength(config);
    }

    function initPasswordStrength(config) {
        let options = {
            common: {
                onLoad: function(){
                    $(inputSelector + '-progress').hide();
                },
                onKeyUp: function(){
                    $(inputSelector + '-progress').show();
                },
            },
            rules: {},
            ui: {
                container: ".js-pw-strength-indicator",
                showVerdictsInsideProgressBar: true,
                progressBarExtraCssClasses: 'js-pw-strength-progress active',
                colorClasses: ["danger", "danger", "warning", "warning", "success", "success"]
            }
        };

        if(config.common.minChar)
            options.common.minChar = config.common.minChar;

        if (config.i18n) {
            options.i18n = {
                t: function(key) {
                    return config.i18n[key];
                }
            };
        }
            

        $inputs.pwstrength(options);
    }
}(jQuery));

export default PwStrength;


