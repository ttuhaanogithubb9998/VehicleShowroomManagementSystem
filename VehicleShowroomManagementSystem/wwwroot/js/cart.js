

$(document).ready(() => {

    let addCart = $("#addCart");
    let minusQuantity = $('.minus-quantity')
    let plusQuantity = $('.plus-quantity')
    let buy = $('#buy-product')

    if (buy.length > 0) {
        let customer = document.cookie.slice(9);
        buy.click(e => {

            if (customer.length <= 0) {
                e.preventDefault();
                $('#loginModal').modal('show');
            }
        })
    }



    if (addCart.length > 0) {

        addCart.click((e) => {
            let element;
            if (e.target.childElementCount == 0) {
                element = e.target.parentElement
            } else {
                element = e.target
            }
            let Id = element.attributes.dataid.value;
            let Customer = document.cookie.slice(9);

            if (Customer.length > 0) {
                $.ajax({
                    url: "/Carts/Add",
                    type: 'post',
                    data: { Id, Customer },
                    success: res => {
                        console.log(res);
                        $('#quantiyCart span').html(res.quantityCart)

                    },
                    error: res => {
                        console.log(res)
                    }
                });


                let listBtn = element.parentElement;
                listBtn.style.position = "relative";
                if (!$(listBtn).children()[2]) {
                    $(listBtn).append($(listBtn).children()[1].outerHTML);
                    console.log();

                    let style = listBtn.querySelectorAll("button")[1].style;
                    style.position = "absolute";
                    style.bottom = "0";
                    style.right = "0";
                    style.transition = "0.3s";
                    setTimeout(function () {
                        style.opacity = "0";
                        style.transform = "translate(40px,-50px) scale(0.5,0.5)";

                        setTimeout(function () {
                            listBtn.querySelectorAll("button")[1].remove();
                        }, 300);
                    }, 0);
                }

            } else {
                $('#loginModal').modal('show');
            }

        })
    }


    if (minusQuantity.length > 0) {
        minusQuantity.click((e) => {

            let cartId = e.target.parentElement.parentElement.children[1].attributes.dataid.value;

            $.ajax({
                url: "/Carts/MinusQuantity",
                data: { cartId },
                type: "post",
                success: res => {

                    console.log(res);

                    if (res.code == 200) {

                        // set quantity vs unit price
                        let qtt = e.target.parentElement.parentElement.children[1];
                        let liElement = $(qtt).closest('.list-group-item')

                        qtt.textContent = res.cart.quantity;
                        liElement.find(".unitPrice")[0].innerText = res.cart.quantity * liElement.find('.price')[0].outerText.slice(0, -2) + " $";




                        // checkCart
                        if (res.inventoryNumber >= res.cart.quantity) {

                            liElement.find(".checkCart")[0].innerHTML = `<input type="hidden" value="${res.cart.id}" />
                                                                         <input type="checkbox" />`
                        }
                        else {
                            liElement.find(".checkCart")[0].innerHTML = `<span class="text-danger"> ${res.cart.quantity}/${res.inventoryNumber}</span>`
                        }


                    } else if (res.code == 300) {

                        //remove

                        let liElement = $(e.target.parentElement.parentElement.children[1]).closest('.list-group-item').remove();

                    } else { alert("error!") };
                },
                error: res => {
                    console.log(res)
                }
            })

        })
    }


    if (plusQuantity.length > 0) {
        plusQuantity.click((e) => {

            let cartId = e.target.parentElement.parentElement.children[1].attributes.dataid.value;

            $.ajax({
                url: "/Carts/PlusQuantity",
                data: { cartId },
                type: "get",
                success: res => {
                    console.log(res);

                    if (res.code == 200) {

                        // set quantity vs unit price
                        let qtt = e.target.parentElement.parentElement.children[1];
                        let liElement = $(qtt).closest('.list-group-item')

                        qtt.textContent = res.cart.quantity;
                        liElement.find(".unitPrice")[0].innerText = res.cart.quantity * liElement.find('.price')[0].outerText.slice(0, -2) + " $";

                    } else if (res.code == 300) {
                        alert("max!")
                    } else { alert("error!") };


                },
                error: res => {
                    console.log(res)
                }
            })
        })
    }


})