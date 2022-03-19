
$(document).ready(() => {

    $('.team-active .slick-track').css("display","block")



    $('#loginModal button').click((e) => {
        e.preventDefault();

        let name = $("#login-name").val();
        let password = $("#login-password").val();

        $.ajax({
            url: "/Customers/Login",
            type: "post",
            data: { account: name, password: password },
            success: function (data) {
                console.log(data);
                if (data.code == 200) {

                    // display avartar customer
                    $('li.header-top-login').html(
                        `<a style="display:block" href="/Customers/Index">
                            <img src="/image/avatar/customer/${data.customer.avatar}" style="height:14px;margin:0 5px" />
                            ${data.customer.fullName}
                         </a>`
                    );


                    // quantity cart customer
                    $(
                        `<li id="quantiyCart" class="header-shop-cart">
                            <a href = "/Customers/Index">
                                <i class="fa fa-shopping-basket"></i><span>${data.quantityProduct}</span>
                            </a>
                        </li>`
                    ).insertBefore('#getaquocte');




                    // modal hide
                    $('#loginModal').modal('hide');
                    $('.modal-backdrop').remove();
                    $("[data-dismiss=modal]").trigger({ type: "click" });
                    $('body').css("padding", '0')
                    $('body').removeClass('modal-open');
                }
                else {
                    // msg 
                    $('#loginModal span').html(`${data.msg}`);
                }
            },
            error: function (res) {
                console.log(res);
            }
        })
    })





})