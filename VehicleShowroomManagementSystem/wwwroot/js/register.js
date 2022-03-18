var loadFile = (e) => {

    let outputImg = document.getElementById('outputImg');

    outputImg.src = URL.createObjectURL(e.target.files[0]);

    outputImg.onload = () => {
        URL.revokeObjectURL(outputImg.src);
    }
}

$(document).ready(function () {

    let inputImg = $('#registration #AvatarFile');
    inputImg.change(loadFile)




    $.validator.addMethod('phone', function (value, element) {

        let Regex = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im;
        //return (Regex.test(value));
        if (Regex.test(value)) {
            return true;   // PASS validation when REGEX matches
        } else {
            return false;  // FAIL validation
        };

    }, "The field is not phone"),

        $("#registration").validate({
            rules: {
                Account: {
                    required: true,
                    minlength: 6
                },
                Password: {
                    required: true,
                    minlength: 6
                },
                password_confirmation: {
                    required: true,
                    minlength: 6,
                    equalTo: "#Password",
                },
                FullName: "required",
                Address: "required",
                NumberPhone: {
                    required: true,
                    phone: true,
                },
                Email: {
                    required: true,
                    email: true
                },
                AvatarFile: "required",


            },

            submitHandler: function (form) {

                let mf = document.getElementById("registration");

                let dt = new FormData(mf);

                $.ajax({
                    url: "/Customers/Register",
                    type: "post",
                    data: dt,
                    processData: false,
                    contentType: false,
                    success: function (data) {

                        console.log(data)
                        if (data.code == 200) {

                            alert(data.msg)

                            $('li.header-top-login').html(
                                `<a style="display:block" href="/Customers/Index">
                            <img src="/image/avatar/customer/${data.customer.avatar}" style="height:14px;margin:0 5px" />
                            ${data.customer.fullName}
                         </a>`
                            );

                            // quantity cart customer
                            $(
                                `<li id="quantiyCart" class="header-shop-cart">
                                <a href = "Customers/Index">
                                    <i class="fa fa-shopping-basket"></i><span>0</span>
                                </a>
                            </li>`
                            ).insertBefore('#getaquocte');



                            $('.modal-backdrop').remove();
                            $("[data-dismiss=modal]").trigger({ type: "click" });
                            $('body').css("padding", '0')
                            $('body').removeClass('modal-open');

                        } else {
                            $('.text-msg').html(data.msg)
                        }

                    },
                    error: function (res) {
                        console.log(res);
                    }
                })






            }
        });
});