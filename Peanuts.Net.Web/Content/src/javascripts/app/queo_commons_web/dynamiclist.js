$(function () {

    $(".dynamic-list").each(function () {

        var dynamicListControl = this;
        
        /* add input element field group */
        $(dynamicListControl).on('click', '.add-list-item', function (e) {
            e.preventDefault();

            var $target = $(e.target);
            var $itemAdder = $target.parent();

            /*Load Template for empty item*/
            var emptyItemTemplate;
            if ($itemAdder.data('empty-item-template') && $itemAdder.data('empty-item-template').length > 0) {
                emptyItemTemplate = $itemAdder.data('empty-item-template');
            } else {
                emptyItemTemplate = $(dynamicListControl).data('empty-item-template');
            }

            /*Index für Binding ersetzen*/
            emptyItemTemplate = emptyItemTemplate.split('xxx_index_for_binding_xxx').join('index_value_' + Date.now());


            var addItemEvent = jQuery.Event("dynamic-list:addItem");
            addItemEvent.item = emptyItemTemplate;
            $(dynamicListControl).trigger(addItemEvent);

            console.log("Neuer Eintrag soll eingefügt werden");

            if (addItemEvent.isDefaultPrevented()) {

            } else {

                console.log("Neuer Eintrag wird eingefügt");
                $(this).parent().before(emptyItemTemplate);
                var itemAddedEvent = jQuery.Event("dynamic-list:itemAdded");
                itemAddedEvent.item = $(this).parent().prev();
                $(dynamicListControl).trigger(itemAddedEvent);
                console.log("Neuer Eintrag wurde eingefügt");
            }
        });

        /* remove input element field group */
        $(dynamicListControl).on('click', '.remove-list-item', function (e) {
            e.preventDefault();
            var removeDirectly = $(this).data("remove-directly") == "True" || $(this).data("remove-directly") == "true" || $(this).data("remove-directly") == true;
            var listItemToRemove = $(this).closest('.dynamic-list-item');
            console.log("Item aus Liste löschen.");
            var removeItemEvent = jQuery.Event("dynamic-list:removeItem");
            removeItemEvent.item = listItemToRemove;
            $(dynamicListControl).trigger(removeItemEvent);
            console.log("DefaultPrevented: " + removeItemEvent.isDefaultPrevented());
            if (removeItemEvent.isDefaultPrevented()) {
            } else {
                if (removeDirectly) {
                    listItemToRemove.remove();
                    $(dynamicListControl).trigger("dynamic-list:itemRemoved");
                } else {
                    $(listItemToRemove).toggleClass("trash");
                    $(".deleteMarker", $(listItemToRemove)).val($(listItemToRemove).hasClass("trash"));
                    if ($(listItemToRemove).hasClass("trash")) {
                        $(dynamicListControl).trigger("dynamic-list:itemRemoved");
                    }
                }
            }
        });
    });


    $(".dynamic-text-list").each(function (index, dynamicTextListControl) {

        $(function () {
            /* add input element field group */
            $(dynamicTextListControl).on('click', '.add-text-list-item', function (e) {
                e.preventDefault();

                var emptyItemControl = $(dynamicTextListControl).data('empty-item-template');
                var addItemEvent = jQuery.Event("dynamic-text-list:addItem");
                addItemEvent.item = emptyItemControl;
                $(dynamicTextListControl).trigger(addItemEvent);

                console.log("Neuer Eintrag soll eingefügt werden");

                if (addItemEvent.isDefaultPrevented()) {

                } else {
                    $(this).parent().before(emptyItemControl);
                    var itemAddedEvent = jQuery.Event("dynamic-text-list:itemAdded");
                    itemAddedEvent.item = $(this).parent().prev();
                    $(dynamicTextListControl).trigger(itemAddedEvent);
                    console.log("Neuer Eintrag wurde eingefügt");
                }
            });

            /* remove input element field group */
            $(dynamicTextListControl).on('click', '.remove-text-list-item', function (e) {
                e.preventDefault();
                console.log("item soll entfernt werden");
                var listItemToRemove = $(this).parent('.dynamic-text-list-item');
                var removeItemEvent = jQuery.Event("dynamic-text-list:removeItem");
                removeItemEvent.item = listItemToRemove;
                $(dynamicTextListControl).trigger(removeItemEvent);
                if (removeItemEvent.isDefaultPrevented()) {
                } else {
                    listItemToRemove.remove();
                    $(dynamicTextListControl).trigger("dynamic-text-list:itemRemoved");
                }
            });
        });


    });

});