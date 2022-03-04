
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
                $('li.header-top-login').html(`<a style="display:block" href="/Customers/Index">
                                                    <img src="~/image/avatar/customer/${data.customer.avatar}" style="height:14px;margin:0 5px" />
                                                 </a>`)
                $('.modal-backdrop').remove();

            },
            error: function (res) {
                console.log(res);
            }
        })
    })

})