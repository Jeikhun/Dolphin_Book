
//quantity box
function wcqib_refresh_quantity_increments() {
    jQuery("div.quantity:not(.buttons_added), td.quantity:not(.buttons_added)").each(function(a, b) {
        var c = jQuery(b);
        c.addClass("buttons_added"), c.children().first().before('<input type="button" value="-" class="minus" />'), c.children().last().after('<input type="button" value="+" class="plus" />')
    })
}
String.prototype.getDecimals || (String.prototype.getDecimals = function() {
    var a = this,
        b = ("" + a).match(/(?:\.(\d+))?(?:[eE]([+-]?\d+))?$/);
    return b ? Math.max(0, (b[1] ? b[1].length : 0) - (b[2] ? +b[2] : 0)) : 0
}), jQuery(document).ready(function() {
    wcqib_refresh_quantity_increments()
}), jQuery(document).on("updated_wc_div", function() {
    wcqib_refresh_quantity_increments()
}), jQuery(document).on("click", ".plus, .minus", function() {
    var a = jQuery(this).closest(".quantity").find(".qty"),
        b = parseFloat(a.val()),
        c = parseFloat(a.attr("max")),
        d = parseFloat(a.attr("min")),
        e = a.attr("step");
    b && "" !== b && "NaN" !== b || (b = 0), "" !== c && "NaN" !== c || (c = ""), "" !== d && "NaN" !== d || (d = 0), "any" !== e && "" !== e && void 0 !== e && "NaN" !== parseFloat(e) || (e = 1), jQuery(this).is(".plus") ? c && b >= c ? a.val(c) : a.val((b + parseFloat(e)).toFixed(e.getDecimals())) : d && b <= d ? a.val(d) : b > 0 && a.val((b - parseFloat(e)).toFixed(e.getDecimals())), a.trigger("change")
});



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


    $(document).on('click', '#increaseCount', function (e) {
        e.preventDefault();

        var increaseCount = $(this);

        var id = $(this).data('id');

        $.ajax({
            method: "POST",
            url: "/basket/increasecount",
            data: {
                id: id,
                type: $(this).attr('data-type')
            },
            success: function (res) {
                window.location.reload();
                
            },
            error: function (err) {
                alert(err.responseText)
                window.location.reload();
            }
        })
    })

    $(document).on('click', '#decreaseCount', function (e) {
        e.preventDefault();

        var decreaseCount = $(this);

        var id = $(this).data('id');

        $.ajax({
            method: "POST",
            url: "/basket/decreasecount",
            data: {
                id: id,
                type: $(this).attr('data-type')
            },
            success: function (res) {
                window.location.reload();
                var countElement = $(decreaseCount).parent().siblings().eq(3);
                var count = parseInt(countElement.html());

                if (count > 0) {
                    count--;
                    countElement.html(count);
                }
            },
            error: function (err) {
                alert(err.responseText)
                window.location.reload();
            }
        })
    })

    $(document).on('click', '#removeProduct', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        $.ajax({
            method: "POST",
            url: "basket/Remove",
            data: {
                id: id
            },
            success: function () {
                $(`.table tr[id=${id}]`).remove();
            },
            error: function (err) {
                alert(err.responseText)
            }
        })
    })



})

jQuery(function ($) {
    $(document).on('click', '#addToyToBasket', function () {

        var id = $(this).data('id');

        $.ajax({
            method: "POST",
            url: "/basket/addToy",
            data: {
                id: id
            },
            success: function (result) {
                console.log(result);
            }

        })

    })


})

//const btnAddToBasket = document.querySelector("#addBookToBasket");
//btnAddToBasket.addEventListener("click", (e) => {

//    window.location.reload();
//    console.log("2")

//})
function displayRadioValue() {
    var totalPrice = parseFloat(document.querySelector("#book-total-price").textContent);
    
    console.log(totalPrice);
    var radio2 = parseFloat(document.querySelector("input[name=shipping_method]:checked").parentElement.lastElementChild.firstElementChild.textContent);
    if (isNaN(radio2)) {
        radio2 = 0;
    }
    console.log(radio2);
    const subTotalPrice = totalPrice + radio2;
    document.getElementById("total-price-span").innerHTML
        = subTotalPrice;
}