$(document).ready(function () {
    debugger;
    $.ajax({
        url: '/ProviderPages/SpNewRequest',
        type: 'GET',
        success: function (response) {

            $('#v-pills-tabContent').html(response);
            $('#SPexample').DataTable({
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

function loadSPdb() {
    debugger;
    $.ajax({
        url: '/ProviderPages/SpNewRequest',
        type: 'GET',
        success: function (response) {
            $('#v-pills-tabContent').html(response);
            $('#SPexample').DataTable({
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

function loadSPupcomingsrvice() {
    debugger;
    $.ajax({
        url: '/ProviderPages/SpUpcomingRequest',
        type: 'GET',
        success: function (response) {
            $('#v-pills-tabContent').html(response);
            $('#SPexample1').DataTable({
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

function loadSPhistory() {
    debugger;
    $.ajax({
        url: '/ProviderPages/SpHistory',
        type: 'GET',
        success: function (response) {
            $('#v-pills-tabContent').html(response);
            $('#SPexample2').DataTable({
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
function loadSPrating() {
    debugger;
    $.ajax({
        url: '/ProviderPages/SpRating',
        type: 'GET',
        success: function (response) {
            $('#v-pills-tabContent').html(response);
        }
    });
}

function loadSPblock() {
    debugger;
    $.ajax({
        url: '/ProviderPages/SpBlock',
        type: 'GET',
        success: function (response) {
            $('#v-pills-tabContent').html(response);
        }
    });
}
function loadSPsetting() {
    debugger;
    $.ajax({
        url: '/ProviderPages/SpSetting',
        type: 'GET',
        success: function (response) {
            $('#v-pills-tabContent').html(response);
        }
    });
}

function opennewreq(or) {
    debugger;
    var id = or.getAttribute("data-id");
    $.ajax({
        url: '/ProviderPages/OpenNewServiceModel',
        type: 'GET',
        data: { "id": id },
        success: function (response) {
            $('#1Model').html(response);
            $('#ServiceDetailesSP').modal("show");
        }
    });
}
function openupcomingreq(or) {
    debugger;
    var id = or.getAttribute("data-id");
    $.ajax({
        url: '/ProviderPages/OpenUpComingModel',
        type: 'GET',
        data: { "id": id },
        success: function (response) {
            $('#2Model').html(response);
            $('#UpcomingServiceDetailesSP').modal("show");
        }
    });
}
function openhistory(or) {
    debugger;
    var id = or.getAttribute("data-id");
    $.ajax({
        url: '/ProviderPages/OpenHistoryModel',
        type: 'GET',
        data: { "id": id },
        success: function (response) {
            $('#3Model').html(response);
            $('#ServiceHistorySP').modal("show");
        }
    });
}

function acceptreq(cm) {
    debugger;
    var id = cm.getAttribute("data-id");
    $.ajax({
        url: '/ProviderPages/AcceptNewRequest',
        type: 'POST',
        data: { "id": id },
        success: function (result) {
            if (result.value == "true") {
                loadSPdb();
            }
            else {
                alert("Invalid");
            }
        }
    });
}
function cancelreq(cm) {
    debugger;
    var id = cm.getAttribute("data-id");
    $.ajax({
        url: '/ProviderPages/CancelRequest',
        type: 'POST',
        data: { "id": id },
        success: function (result) {
            if (result.value == "true") {
                loadSPdb();
            }
            else {
                alert("Invalid");
            }
        }
    });
}
function completereq(cm) {
    debugger;
    var id = cm.getAttribute("data-id");
    $.ajax({
        url: '/ProviderPages/CompleteRequest',
        type: 'POST',
        data: { "id": id },
        success: function (result) {
            if (result.value == "true") {
                loadSPdb();
            }
            else {
                alert("Invalid");
            }
        }
    });
}

function updatespdetails() {
    debugger;
    var data = {};
    data.firstName = document.getElementById("firstname").value;
    data.lastName = document.getElementById("lastname").value;
    data.mobile = document.getElementById("mobile").value;
    data.day = document.getElementById("birthdate").value;
    data.month = document.getElementById("birthmonth").value;
    data.year = document.getElementById("birthyear").value;
    data.nationalityId = document.getElementById("nationalityId").value;
    data.gender = document.getElementById("Gender").value;
    data.addressLine1 = document.getElementById("Housenumber").value;
    data.addressLine2 = document.getElementById("Street").value;
    data.city = document.getElementById("city").value;
    data.postalCode = document.getElementById("postalcode").value;

    console.log("get detailes new");
    $.ajax({
        type: 'POST',
        url: '/ProviderPages/Updateproviderdetails',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: data,
        success: function (result) {
            if (result.value == "true") {
                loadSPsetting();
            }
            else {
                alert("detailes is invalid");
            }
        }
    })
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
                error: function (response) {
                    alert("Invalid password");
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

function blocksp(sp) {
    debugger;
    var id = sp.getAttribute("data-id");
    $.ajax({
        url: '/ProviderPages/BlockUser',
        type: 'POST',
        contenttype: 'application/json',
        data: { "id": id },
        success: function () {
            loadblockcustomer();
        }
    });
}
function unblocksp(sp) {
    debugger;
    var id = sp.getAttribute("data-id");
    $.ajax({
        url: '/ProviderPages/UnBlockUser',
        type: 'POST',
        contenttype: 'application/json',
        data: { "id": id },
        success: function () {
            loadblockcustomer();
        }
    });
}


//for downloading the excel file
function ExportToExcel(type, fn, dl) {
    var excel = document.getElementById('SPexample2');
    var wb = XLSX.utils.table_to_book(excel, { sheet: "sheet1" });
    return dl ?
        XLSX.write(wb, { bookType: type, bookSST: true, type: 'base64' }) :
        XLSX.writeFile(wb, fn || ('Servicehistory.' + (type || 'xlsx')));
}
 
    $(document).ready(function () {
        $("#change-password form").data("validator", null);
    $.validator.unobtrusive.parse("#change-password form");
   });
 