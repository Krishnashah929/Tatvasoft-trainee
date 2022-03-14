
$(document).ready(function () {

        $.ajax({
            url: '/CustomerPages/Loaddashboard',
            type: 'GET',
            success: function (response) {

                $('#v-pills-tabContent').html(response);
                $('#example').DataTable({
                    "searching": false,
                    //  "dom": '<"top"i>rt<"bottom"flp><"clear">',
                    columnDefs: [{
                        targets: [0],
                        orderData: [0, 1]
                    }, {
                        targets: [1],
                        orderData: [1, 0]
                    }, {
                        targets: [4],
                        orderData: [4, 0]
                    }]
                });

            }
        });
      });
function loadhistory() {
        $.ajax({
            url: '/CustomerPages/history',
            type: 'GET',
            success: function (response) {
                $('#v-pills-tabContent').html(response);
                $('#example2').DataTable({
                    "searching": false,
                    //  "dom": '<"top"i>rt<"bottom"flp><"clear">',
                    columnDefs: [{
                        targets: [0],
                        orderData: [0, 1]
                    }, {
                        targets: [1],
                        orderData: [1, 0]
                    }, {
                        targets: [4],
                        orderData: [4, 0]
                    }]
                });
            }
        });
      }
function loaddb() {
        
        $.ajax({
            url: '/CustomerPages/Loaddashboard',
            type: 'GET',
            success: function (response) {
                $('#v-pills-tabContent').html(response);
                $('#example').DataTable({
                    "searching": false,
                    //  "dom": '<"top"i>rt<"bottom"flp><"clear">',
                    columnDefs: [{
                        targets: [0],
                        orderData: [0, 1]
                    }, {
                        targets: [1],
                        orderData: [1, 0]
                    }, {
                        targets: [4],
                        orderData: [4, 0]
                    }]
                });
            }
        });
      }
function loadsetting() {
        $.ajax({
            url: '/CustomerPages/setting',
            type: 'GET',
            success: function (response) {
                $('#v-pills-tabContent').html(response);

            }
        });
      }
function ServiceDetails(sd) {
    var id = sd.getAttribute("data-id");
    $.ajax({
        url: '/CustomerPages/ServiceModel',
        type: 'GET',
        data: { "id": id },
        success: function (response) {
            $('#Model1').html(response);
            $('#ServiceDetailes').modal("show");
        }
    });
}
function reschedule(sd) {
    var id = sd.getAttribute("data-id");
    $.ajax({
        url: '/CustomerPages/RescheduleServiceModel',
        type: 'GET',
        data: { "id": id },
        success: function (response) {
            $('#Model2').html(response);
            $('#RescheduleModal').modal("show");
        }
    });
}
function changeschedule() {
    var data = {};
    data.serviceRequestId = document.getElementById("id").value;
    data.serviceDate = document.getElementById("Date").value;
    data.serviceTime = document.getElementById("Time").value;
    $.ajax({
        url: '/CustomerPages/UpdateRescheduleeModel',
        type: 'POST',
        data: data,
        success: function (result) {
            if (result.value == "true") {
                loaddb();
            }
            else {
                alert("Invalid");
            }
        }
    });
}
function cancel(cm) {
    var id = cm.getAttribute("data-id");
    $.ajax({
        url: '/CustomerPages/CancelServiceModel',
        type: 'GET',
        data: { "id": id },
        success: function (response) {
            $('#Model3').html(response);
            $('#CancelModal').modal("show");
        }
    });
}
function CancelRequest() {
    var data = {};
    data.serviceRequestId = document.getElementById("id").value;
    data.comments = document.getElementById("cancelrequest").value;
    $.ajax({
        url: '/CustomerPages/CancelRequestModel',
        type: 'POST',
        data: data,
        success: function (result) {
            if (result.value == "true") {
                loaddb();
            }
            else {
                alert("Invalid");
            }
        }
    });
}
function rating(cm) {
    debugger;
    var id = cm.getAttribute("data-id");
    $.ajax({
        url: '/CustomerPages/RatingProviderModel',
        type: 'GET',
        data: { "id": id },
        success: function (response) {
            $('#Model4').html(response);
            $('#RatingModel').modal("show");
        }
    });
}
function saverating() {
    debugger;
    var data = {};
    data.serviceRequestId = document.getElementById("id").value;
    $.ajax({
        url: '/CustomerPages/AddRatings',
        type: 'POST',
        data: data,
        success: function (result) {
            if (result.value == "true") {
                loadhistory();
            }
            else {
                alert("Invalid");
            }
        }
    });
}
function updatedetails() {
    debugger;
    var data = {};
    data.firstName = document.getElementById("firstname").value;
    data.lastName = document.getElementById("lastname").value;
    data.day = document.getElementById("birthdate").value;
    data.month = document.getElementById("birthmonth").value;
    data.year = document.getElementById("birthyear").value;
    data.mobile = document.getElementById("mobile").value;
    data.languageId = document.getElementById("Language").value;
    console.log("get detailes new");
    $.ajax({
        type: 'POST',
        url: '/CustomerPages/Updateuserdetails',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: data,
        success: function (result) {
            if (result.value == "true") {
                loadsetting();
            }
            else {
                alert("detailes is invalid");
            }
        }
    })
}
function editadd(ed) {
    debugger;
    var id = ed.getAttribute("data-id");
    $.ajax({
        url: '/CustomerPages/EditAddressModel',
        type: 'GET',
        data: { "id": id },
        success: function (response) {
            $('#Model5').html(response);
            $('#myModalforaddress').modal("show");
        }
    });
}
function deladd(da) {
    var id = da.getAttribute("data-id");
    $.ajax({
        url: '/CustomerPages/DelAddressModel',
        type: 'GET',
        data: { "id": id },
        success: function (response) {
            $('#Model5').html(response);
            $('#myModaldeleteaddress').modal("show");
        }
    });
}
function newadd() {
    var data = {};
    data.addressId = document.getElementById("id").value;
    data.addressLine1 = document.getElementById("Street").value;
    data.addressLine2 = document.getElementById("number").value;
    data.postalCode = document.getElementById("PostalCode").value;
    data.state = document.getElementById("state").value;
    data.mobile = document.getElementById("Mobile").value;
    console.log("get detailes new");
    $.ajax({
        type: 'POST',
        url: '/CustomerPages/UpdateAddress',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: data,
        success: function (result) {
            if (result.value == "true") {
                loadsetting();
            }
            else {
                alert("detailes is invalid");
            }
        }
    })
}
function delexistadd() {
    var data = {};
    data.addressId = document.getElementById("id").value;
    $.ajax({
        url: '/CustomerPages/DeleteAddress',
        type: 'POST',
        data: data,
        success: function (result) {
            if (result.value == "true") {
                loadsetting();
            }
            else {
                alert("Invalid");
            }
        }
    });
}
function updatepw() {
    debugger;
    var storepassword = $("#storepassword").val();
    var oldpass = $("#oldpassword").val();
    var newpass = $("#newpassword").val();
    var confirmpass = $("#repeatpassword").val();
    if (storepassword != oldpass) {
        alert("You have entered wrong current password !");
    }
    if (oldpass != "" && newpass != "" && confirmpass != "") {
        if (newpass == confirmpass) {
            var model = {
                Password: oldpass,
                NewPassword: newpass,
            }
            $.ajax({
                type: 'POST',
                url: '/ProviderPages/ChangePassword',
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                data: model,
                success: function (response) {
                    alert("valid password");
                },
            });
        }
        else {
            alert("Password does not match .");
        }
    }
    else {
        alert("Null values are not accepted ! ");
    }
}

//for downloading the excel file
function ExportToExcelCust(type, fn, dl) {
    var elt = document.getElementById('example2');
    var wb = XLSX.utils.table_to_book(elt, { sheet: "sheet1" });
    return dl ?
        XLSX.write(wb, { bookType: type, bookSST: true, type: 'base64' }) :
        XLSX.writeFile(wb, fn || ('Servicehistorycustomer.' + (type || 'xlsx')));
}