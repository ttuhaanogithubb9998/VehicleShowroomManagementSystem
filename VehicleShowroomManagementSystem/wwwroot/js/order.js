
$(document).ready(() => {

    //
    $('#Order').click((e) => {
        e.preventDefault();
        let listCheck = $('.list-cart input:checked').parent(".checkCart").children("input:hidden");
        console.log(listCheck)
        var values = '';
        listCheck.map((i, e) => {
            if (i == 0) {
                values += e.value;
            } else {
                values += (',' + e.value)
            }
        })

        console.log(values)

        if (values.length > 0) {

            $.ajax({
                url: "/Carts/Order",
                type: "post",
                data: { values },
                success: (res) => {
                    console.log(res)
                    $(".customer").html(res);
                    order();
                },
                error: (res) => {
                    console.log(res)
                }
            })
        }
    })

    let order = () => {
        let formOrder = $("#form-order")
        if (formOrder.length > 0) {
            formOrder.validate({
                rules: {
                    ShippingAddress: {
                        requied: true
                    },
                    ShippingPhone: {
                        requied:true
                    }
                },

                submitHandler: () => {
                    alert();
                }
            })
        }
    }








})