

// Toast Message

var toast = document.querySelector(".toast");
var btn = document.querySelectorAll(".toast-btn");
var close = document.querySelector(".toast-close");
var text2 = document.querySelector(".text-2");
var text1 = document.querySelector(".text-1");
var toastCheck = document.querySelector(".toast-check");

var progress = document.querySelector(".progress");

close.addEventListener("click", () => {
    toast.classList.remove("active");
    setTimeout(() => {
        progress.classList.remove("active");
    }, 300)
})



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
                toastCheck.classList.remove("bi-exclamation-lg")
                toastCheck.classList.add("bi-check2");
                toastCheck.style.backgroundColor = "#40f467";
                text1.textContent = "Success";
                text2.textContent = "Məhsul Uğurla Əlavə edildi...";
                toast.style.borderLeft = "8px solid #40f467";
                        toast.classList.add("active");
                        progress.classList.add("active");
                progress.style.background = "#40f467";

                        setTimeout(() => {
                            toast.classList.remove("active");
                        }, 1500)

                        setTimeout(() => {
                            progress.classList.remove("active");
                            location.reload();
                        }, 1700)

            },
            error: function (err) {

                toastCheck.classList.remove("bi-check2")
                toastCheck.classList.add("bi-exclamation-lg");
                toastCheck.style.backgroundColor = "red";
                toast.style.borderLeft = "8px solid red";
                text1.textContent = "Unsuccess";
                text2.textContent = "Stokda Əlavə Məhsul Yoxdur..."
                toast.classList.add("active");
                progress.classList.add("active");
                progress.style.background = "#red";

                setTimeout(() => {
                    toast.classList.remove("active");
                }, 1500)

                setTimeout(() => {
                    progress.classList.remove("active");
                }, 1700)
            }

        })

    })


});