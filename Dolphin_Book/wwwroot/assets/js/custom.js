jQuery(function ($) {
    $(document).on('click', '#addToBookBasket', function () {

        var id = $(this).data('id');
        var count = $(this).data('count');

        $.ajax({
            method: "POST",
            url: "/basket/addBasket",
            data: {
                id: id,
                type: $(this).attr('data-type'),
                stockCount: count
            },
            success: function (result) {
                window.location.reload();
            }

        })

    })
})