
$(document).ready(() => {
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
                    $('li.header-top-login').html(
                        `<a style="display:block" href="/Customers/Index">
                            <img src="/image/avatar/customer/${data.customer.Avatar}" style="height:14px;margin:0 5px" />
                            ${data.customer.FullName}
                         </a>`
                    );

                    $('#loginModal').modal('hide');
                    $('.modal-backdrop').remove();
                    $("[data-dismiss=modal]").trigger({ type: "click" });
                    $('body').removeClass('modal-open');
                }
                else {
                    $('#loginModal span').html(`${data.msg}`);
                }
            },
            error: function (res) {
                console.log(res);
            }
        })
    })





})