
let burgerMenuList = document.querySelector(".burger-menu-list")
let burgerIcon = document.querySelector(".burger-icon");
let burgerCloseBtn = document.querySelector(".burger-close-btn");
burgerIcon.addEventListener("click", () => {

    burgerMenuList.style.left = "0px"

})
document.s
burgerCloseBtn.addEventListener("click", () => {

    burgerMenuList.style.left = "-600px"

})

let clientInfo = document.querySelector(".client-info");
let personIcon = document.querySelector(".bi-person");
personIcon.addEventListener("click", (e) => {
    if (clientInfo.style.display == "initial") {

        clientInfo.style.display = "none";
    } else {

        clientInfo.style.display = "initial";
        basketContainer.style.display = "none";
        e.stopPropagation();
    }
})

let accountBtn = document.querySelector(".account");

accountBtn.addEventListener("click", (e) => {
    if (clientInfo.style.display == "initial") {

        clientInfo.style.display = "none";
    } else {

        clientInfo.style.display = "initial";
        basketContainer.style.display = "none";
        e.stopPropagation();
    }


})

let basketBtn = document.querySelector(".bi-bag-check");
let basketContainer = document.querySelector(".basket-container");
basketBtn.addEventListener("click", (e) => {
    
    if (window.innerWidth >= 1280 && basketContainer.style.display == "") {

        e.stopPropagation();
        clientInfo.style.display = "none"
        basketContainer.style.display = "flex";

    }
    else if (window.innerWidth >= 1280 && basketContainer.style.display == "none") {
        basketContainer.style.display = "flex";
        clientInfo.style.display = "none";
        e.stopPropagation();
    }
    else if (basketContainer.style.display == "flex") {

        basketContainer.style.display = "none";

    }
})

let closeBasketBtn = document.querySelector(".burger-close-button");
closeBasketBtn.addEventListener("click", () => {

    basketContainer.style.display = "none";
})
if (window.innerWidth < 1280) {

    basketContainer.style.display = "none";
}
let closeWithBody = document.querySelector(".wrapper");
closeWithBody.addEventListener("click", () => {
    console.log("123");
    if (clientInfo.style.display == "initial") {
        clientInfo.style.display = "none";
    }
    if (basketContainer.style.display == "flex") {
        basketContainer.style.display = "none"
    }

});
clientInfo.addEventListener("click", (e) => {
    e.stopPropagation();
})

basketContainer.addEventListener("click", (e) => {
    e.stopPropagation();
})



//toast message
const notifications = document.querySelector(".notifications"),
    buttons = document.querySelectorAll(".toastBTN")

const toastDetails = {
    timer: 5000,
    success: {
        icon: "fa-circle-check",
        text: "Hello World: This is a success toast.",
    },
    error: {

        icon: "fa-circle-xmark",
        text: "Hello World: This is an error toast.",
    },
    warning: {
        icon: "fa-triangle-exclamation",
        text: "Hello World: This is a warning toast.",
    },
    info: {
        icon: "fa-circle-info",
        text: "Hello World: This is an information toast.",
    },
    random: {
        icon: "fa-solid fa-star",
        text: "Hello World: This is a random toast.",
    },
}

const removeToast = (toast) => {
    toast.classList.add("hide")
    if (toast.timeoutId) clearTimeout(toast.timeoutId)
    setTimeout(() => toast.remove(), 500)
}

const createToast = (id) => {
    const { icon, text } = toastDetails[id]
    const toast = document.createElement("li")
    toast.className = `toast ${id}`
    toast.innerHTML = `<div class="column">
                         <i class="fa-solid ${icon}"></i>
                         <span>${text}</span>
                      </div>
                      <i class="fa-solid fa-xmark" onclick="removeToast(this.parentElement)"></i>`
    notifications.appendChild(toast)
    toast.timeoutId = setTimeout(() => removeToast(toast), toastDetails.timer)
}

buttons.forEach((btn) => {
    btn.addEventListener("click", () => {

        console.log("clicked")

        createToast(btn.id)



    })
})