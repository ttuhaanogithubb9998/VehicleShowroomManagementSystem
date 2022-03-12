
$(document).ready(function () {

    let inputImg = $('#AvatarFile');

    function loadFile(e) {
        let outputImg = document.getElementById('outputImg');
        outputImg.src = URl.createObjectURL(e.target.files[0]);
        outputImg.onload = ()=>{
            URL.revokeObjectURL(outputImg.src);
        }
    }

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
                avatar: "required",


            },

            //messages: {
            //    firstname: "Please enter your firstname",
            //    lastname: "Please enter your lastname",
            //    password: {
            //        required: "Please provide a password",
            //        minlength: "Your password must be at least 5 characters long"
            //    },
            //    password_confirmation: {
            //        required: "Please provide a password",
            //        minlength: "Your password must be at least 5 characters long"
            //    },
            //    email: "Please enter a valid email address"
            //},

            submitHandler: function (form) {

                //function getFormData(form) {
                //    var unindexed_array = form.serializeArray();
                //    var indexed_array = {};

                //    $.map(unindexed_array, function (n, i) {
                //        indexed_array[n['name']] = n['value'];
                //    });

                //    return indexed_array;
                //}
                //var data = getFormData($("#registration"));

                let mf = document.getElementById("registration");

                let dt = new FormData(mf);

                //dt.append("Account", $('#registration #Account').val())
                //dt.append("Password", $('#registration #Password').val())
                //dt.append("FullName", $('#registration #FullName').val())
                //dt.append("PhoneNumber", $('#registration #PhoneNumber').val())
                //dt.append("Email", $('#registration #Email').val())
                //dt.append("AvatarFile", $('#registration #AvatarFile').val())

                $.ajax({
                    url: "/Customers/Register",
                    type: "post",
                    data: dt,
                    processData: false,
                    contentType: false,
                    success: function (data) {
                        console.log(data);
                        $('li.header-top-login').html(
                            `<a style="display:block" href="/Customers/Index">
                            <img src="/image/avatar/customer/${data.customer.Avatar}" style="height:14px;margin:0 5px" />
                            ${data.customer.FullName}
                         </a>`
                        );

                        $('#registerModal form').html(`<p class="text-success text-center">${data.msg}</p>`)

                    },
                    error: function (res) {
                        console.log(res);
                    }
                })


                //$.ajax({
                //    url: "/Customers/Register",
                //    type: "post",
                //    data: dt,
                //    success: (data => {
                //        cconsole.log(data);
                //    }),
                //    error: (res => console.log(res))

                //})


                //for (var pair of dt.entries()) {
                //    console.dir(pair[0] + ', ' + pair[1]);
                //}

                //console.log(mf)
                //console.log('dt', dt);





            }
        });
});