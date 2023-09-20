window.onload = function () {

    var changedColor = localStorage.getItem("changeColor");
    console.log(changedColor);
    var a = document.querySelector(`.navii1`);
    console.log(a)

    // ...
}



let filterChildren = document.querySelector("#filter-children");
let categoryChildren = document.querySelector("#category-children");
categoryChildren.parentElement.firstElementChild.firstElementChild.style.display = "block";
categoryChildren.parentElement.firstElementChild.firstElementChild.style.color = "rgb(12, 69, 245)";
categoryChildren.parentElement.firstElementChild.lastElementChild.firstElementChild.style.display = "none";
categoryChildren.parentElement.firstElementChild.lastElementChild.lastElementChild.style.display = "initial";
filterChildren.parentElement.firstElementChild.firstElementChild.style.display = "block";
filterChildren.parentElement.firstElementChild.firstElementChild.style.color = "rgb(12, 69, 245)";
filterChildren.parentElement.firstElementChild.lastElementChild.firstElementChild.style.display = "none";
filterChildren.parentElement.firstElementChild.lastElementChild.lastElementChild.style.display = "initial";




let plusBtn = document.querySelectorAll(".bi-plus-lg");
let minusBtn = document.querySelectorAll(".bi-dash-lg");
plusBtn.forEach(element => {
    element.addEventListener("click", (e)=>{
        // if(e.target.parentElement.parentElement.parentElement.lastElementChild.style.display=="block" ){

        //     console.log(e.target.parentElement.parentElement.parentElement.lastElementChild);
        //     e.target.parentElement.parentElement.parentElement.lastElementChild.style.display="none";
        //     e.target.parentElement.parentElement.firstElementChild.style.color = "black"
            
        // }
    if(e.target.parentElement.parentElement.parentElement.lastElementChild.style.display=="none"||e.target.parentElement.parentElement.parentElement.lastElementChild.style.display==""){
        console.log("Aaaa");
        e.target.parentElement.parentElement.parentElement.lastElementChild.style.display="block";
        e.target.parentElement.parentElement.firstElementChild.style.color = "rgb(12, 69, 245)"
        e.target.parentElement.lastElementChild.style.display = "initial"
            e.target.style.display = "none"
    }
    });
});


minusBtn.forEach(element => {
    element.addEventListener("click", (e) => {

        if (e.target.parentElement.parentElement.parentElement.lastElementChild.style.display == "block" ||
            e.target.parentElement.parentElement.parentElement.lastElementChild.style.display == "") {
            console.log("clicked");
            e.target.parentElement.parentElement.parentElement.lastElementChild.style.display="none";
            e.target.parentElement.parentElement.firstElementChild.style.color = "black"
            e.target.parentElement.firstElementChild.style.display = "initial"
            e.target.style.display = "none"
        }

    });
});



let searchInput = document.querySelector(".search").firstElementChild;
searchInput.addEventListener("keyup", () => {
    const bookAuthor = document.querySelectorAll("#book-author");
    const bookName = document.querySelectorAll('.book-carousel-detail :nth-child(2)');
    const bookCard = document.querySelectorAll(".books-grid>div")
    const booksGrid = document.querySelector(".books-grid");
    const searchInvalid = document.querySelector("#search-invalid");
    const bookCount = document.querySelector(".search-result>p");
    console.log(bookCard[1]);
    console.log(bookName[1]);
    console.log(bookAuthor.length);
    if (searchInput.value != "") {

        var countForBook = 0;

        for (let i = 0; i < bookAuthor.length; i++) {
            if (bookAuthor[i].textContent.toLowerCase().includes(searchInput.value.toLowerCase()) || bookName[i].textContent.toLowerCase().includes(searchInput.value.toLowerCase())) {
                bookCard[i].style.display = "initial";
                countForBook++;
            } else {
                bookCard[i].style.display = "none";
            }

        }

        console.log(searchInput.value);
        if (countForBook < 1) {

            booksGrid.style.display = "none";
            searchInvalid.style.display = "initial";
            bookCount.textContent = `Nəticə tapılmadı`;
        }
        else {
            booksGrid.style.display = "grid";
            searchInvalid.style.display = "none";
            bookCount.textContent = `${countForBook} nəticə tapıldı`;
        }
    }
    
    
});


jQuery(function ($) {
    $(document).on('click', '#category-span', function () {

        var id = $(this).data('id');

        $.ajax({
            method: "GET",
            url: "/book/index",
            data: {
                Categoryid: id
            },
            success: function (result) {
                console.log("nicat")
            }

        })

    })
    //$(document).ready(function () {
    //    $("#category-span").click(function () {
    //        console.log("1")
    //        var CategoryId = $(this).attr("id");
    //        var id = $(this).data('id');
    //        console.log(CategoryId);
    //        var url = "/index?CategoryId=" + id;
    //        var s = "book";
    //        console.debug("saurabh url", url);
    //        console.log(id);
    //        $.ajax({
    //            url: s + "/index?CategoryId="+ id,
    //            success: function (data) {
    //                console.log("son")
    //            }
    //        });
    //    });
    //});

})


//const categorySpan = document.querySelectorAll("#category-span");

//categorySpan.forEach(element => {
//    element.addEventListener("click", (e) => {

//        let href = `/book/index?CategoryId=${2}`;
//        fetch(href)
//            .then(x => x.json())
//            .then(x => console.log(x))
//            .catch(err => console.log(err))
//    })

//})

const catFilterBtn = document.querySelectorAll(".cat-filter-nav");
//bulaq
let count111 = 0; 
catFilterBtn.forEach(element => {
    count111++;
    element.addEventListener("click", (e) => {
        //console.log(e.target.className);
        console.log(element.length);
        
        e.target.className = `${e.target.className} balabey${count111}`;
        localStorage.setItem("changeColor", `balabey${count111}`);
        //console.log(e.target.className);
        e.target.style.color = "rgb(12, 69, 245)";
    })
})

//pagination 
const pagination = document.querySelector('.pagination')

if (pagination) {
    const paginationNumbers = document.querySelectorAll('.pagination__number')
    let paginationActiveNumber = document.querySelector('.pagination__number--active')
    const paginationNumberIndicator = document.querySelector('.pagination__number-indicator')
    const paginationLeftArrow = document.querySelector('.pagination__arrow:not(.pagination__arrow--right)')
    const paginationRightArrow = document.querySelector('.pagination__arrow--right')

    const postionIndicator = (element) => {
        const paginationRect = pagination.getBoundingClientRect()
        const paddingElement = parseInt(window.getComputedStyle(element, null).getPropertyValue('padding-left'), 10)
        const elementRect = element.getBoundingClientRect()
        paginationNumberIndicator.style.left = `${elementRect.left + paddingElement - paginationRect.left}px`
        paginationNumberIndicator.style.width = `${elementRect.width - paddingElement * 2}px`
        if (element.classList.contains('pagination__number--active')) {
            paginationNumberIndicator.style.opacity = 1
        } else {
            paginationNumberIndicator.style.opacity = .2
        }
    }

    const setActiveNumber = (element) => {
        if (element.classList.contains('pagination__number--active')) return
        element.classList.add('pagination__number--active')
        paginationActiveNumber.classList.remove('pagination__number--active')
        paginationActiveNumber = element
        setArrowState()
    }

    const disableArrow = (arrow, disable) => {
        if (
            (!disable && !arrow.classList.contains('pagination__arrow--disabled')) ||
            (disable && arrow.classList.contains('pagination__arrow--disabled'))
        )
            return
        if (disable) {
            arrow.classList.add('pagination__arrow--disabled')
        } else {
            arrow.classList.remove('pagination__arrow--disabled')
        }
    }

    const setArrowState = () => {
        const previousElement = paginationActiveNumber.previousElementSibling
        const nextElement = paginationActiveNumber.nextElementSibling
        if (previousElement.classList.contains('pagination__number')) {
            disableArrow(paginationLeftArrow, false)
        } else {
            disableArrow(paginationLeftArrow, true)
        }

        if (nextElement.classList.contains('pagination__number')) {
            disableArrow(paginationRightArrow, false)
        } else {
            disableArrow(paginationRightArrow, true)
        }
    }

    paginationLeftArrow.addEventListener('click', () => {
        setActiveNumber(paginationActiveNumber.previousElementSibling)
        postionIndicator(paginationActiveNumber)
    })

    paginationRightArrow.addEventListener('click', () => {
        setActiveNumber(paginationActiveNumber.nextElementSibling)
        postionIndicator(paginationActiveNumber)
    })

    Array.from(paginationNumbers).forEach((element) => {
        element.addEventListener('click', () => {
            setActiveNumber(element)
            postionIndicator(paginationActiveNumber)
        })

        element.addEventListener('mouseover', () => { postionIndicator(element) })

        element.addEventListener('mouseout', () => { postionIndicator(paginationActiveNumber) })
    })

    postionIndicator(paginationActiveNumber)
}




