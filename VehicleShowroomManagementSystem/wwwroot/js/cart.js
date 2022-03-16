

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
            let Id = e.target.attributes.dataid.value;
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