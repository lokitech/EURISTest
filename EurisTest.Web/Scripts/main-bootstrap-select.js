$(document).ready(function ()   
{   
    $('#MultiList').attr('data-live-search', true);  

    $('#MultiList').attr('multiple', true);  
    $('#MultiList').attr('data-selected-text-format', 'count');  
  
    $('.selectMulti').selectpicker(  
    {  
        title: '- Select Multiple -',   
        iconBase: 'fa',  
        tickIcon: 'fa-check'  
        });

    $('#catalogMultiList').on('changed.bs.select', function (e, clickedIndex, isSelected, previousValue) {
        $('#catalogMultiList').find('option').each(function (index, element) {
            if (index == clickedIndex) {
                if (isSelected) {
                    alert("Catalog " + element.text + " is selected");
                }
                else {
                    alert("Catalog " + element.text + " is de-selected");
                }
            }
        });
        
        e.stopImmediatePropagation()
    });
});

