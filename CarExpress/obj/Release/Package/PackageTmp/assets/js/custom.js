
(function (scope) {
    var StorageService = function () {
        this.localStorage = this.isSupported('localStorage') ? window.localStorage : new CookieStore();
        this.sessionStorage = this.isSupported('sessionStorage') ? window.sessionStorage : new CookieStore(true);
    };
    StorageService.prototype.isSupported = function (type) {
        var testKey = '__isSupported', storage = window[type];
        try {
            storage.setItem(testKey, '1');
            storage.removeItem(testKey);
            return true;
        }
        catch (error) {
            return false;
        }
    };
    var MemoryStore = function () {
        this.store = {};
    };
    MemoryStore.prototype.getItem = function (name) {
        return this.store[name] || null;
    };
    MemoryStore.prototype.setItem = function (name, value) {
        this.store[name] = value;
    };
    MemoryStore.prototype.removeItem = function (name) {
        delete this.store[name];
    };
    var CookieStore = function (isSessionStorage) {
        this.objectStore = {};
        this.expireDate = isSessionStorage ? "; path=/" : "; expires=Tue, 19 Jan 2038 03:14:07 GMT; path=/";
        this.updateObject();
    };
    CookieStore.prototype.getItem = function (name) {
        return this.objectStore[name] || null;
    };
    CookieStore.prototype.setItem = function (name, value) {
        if (!name) { return; }
        document.cookie = escape(name) + "=" + escape(value) + this.expireDate;
        this.updateObject();
    };
    CookieStore.prototype.removeItem = function (name) {
        if (!name) { return; }
        document.cookie = escape(name) + this.expireDate;
        delete this.objectStore[name];
    };
    CookieStore.prototype.updateObject = function () {
        for (var couple, key, i = 0, couples = document.cookie.split(/\s*;\s*/); i < couples.length; i++) {
            couple = couples[i].split(/\s*=\s*/);
            if (couple.length > 1) {
                this.objectStore[key = unescape(couple[0])] = unescape(couple[1]);
            }
        }
    };
    scope.StorageService = new StorageService();
})(window);
var Current_Page_URL = function () {
    return window.location.pathname.replace("/", "").toLowerCase();
}
var isInteger = function (x) {
    return x % 1 === 0;
}
var IsEmailValid = function (email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}
var GetQueryStringParams = function (sParam) {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == sParam) {
            return sParameterName[1];
        }
    }
}
var delay = (function () {
    var timer = 0;
    return function (callback, ms) {
        clearTimeout(timer);
        timer = setTimeout(callback, ms);
    };
})();
var scrollToPosition = function (elem) {
    $('html, body').stop().animate({
        'scrollTop': elem.offset().top - 100
    }, 500, 'swing', function () {
        //window.location.hash = target;

    });
}
var formatDigit = function (n) {
    return n > 9 ? "" + n : "0" + n;
}
var RemoveCharacter = function (output, limit) {
    return output.substr(0, output.length - limit);
}
var GenerateRandomValue = function (length, is_discount) {
    var chars = "abcdefghijklmnopqrstuvwxyz";
    if (!is_discount) {
        chars += "ABCDEFGHIJKLMNOP1234567890";
    }
    var pass = "";
    for (var x = 0; x < length; x++) {
        var i = Math.floor(Math.random() * chars.length);
        pass += chars.charAt(i);
    }
    return pass;
}
$(document).on("click", "a", function () {
    if ($(this).hasClass("disabled")) {
        return false;
    }
});
if (location.hash) {
    location.href = location.hash;
}
var session_variable_name = "premium_session_cart_v1";
var session_expire_time_variable_name = "premium_session_expire_time_v1";
var session_discount_variable_name = "premium_session_discount_v1";
var checkout_page_url = "checkout";
var wishlist_page_url = "wishlist";
var cart_page_url = "cart";
var web_currency = "";
var website_delivery_charges = parseFloat(0);
var website_tax_percentage = parseFloat(0);
var website_minimum_amount = parseFloat(0);
var sub_total = parseFloat(0);
var discount_total = parseFloat(0);
var grand_total = parseFloat(0);
var tax_amount = parseFloat(0);
var web_url = "http://localhost:54871/";
//var web_url = "http://samiacrafts.com/";
var Clear_Storage_Record = function (name) {
    localStorage.removeItem(name);
}
var Add_Storage_Record = function (name, value) {
    localStorage.setItem(name, value);
}
var Get_Storage_Record = function (name) {
    if (localStorage.getItem(name) == null) {
        return null;
    } else {
        return localStorage.getItem(name);
    }
}
var ClearAllStorage = function () {
    Clear_Storage_Record(session_variable_name);
    Clear_Storage_Record(session_expire_time_variable_name);
    Clear_Storage_Record(session_discount_variable_name);
}
var Total_Cart_Price = function () {
    var cart_records = Get_Storage_Record(session_variable_name);
    if (cart_records == null) {
        return parseFloat(0);
    } else {
        var json_array = [];
        var total_price = 0;
        json_array = JSON.parse(cart_records);
        for (var i = 0; i < json_array.length; i++) {
            var data = JSON.parse(json_array[i]);
            total_price += parseFloat((parseInt(data.qty) * parseFloat(data.price)));
            for (var k = 0; k < data.gifts.length; k++) {
                var data1 = data.gifts[k];
                total_price += parseFloat((parseInt(data1.qty) * parseFloat(data1.price)));
            }
        }
        return total_price;
    }
}
var Current_Item_Price = function (id) {
    var item_total_price = parseFloat(0);
    var cart_records = Get_Storage_Record(session_variable_name);
    if (cart_records != null) {
        var json_array = [];
        json_array = JSON.parse(cart_records);
        for (var i = 0; i < json_array.length; i++) {
            var data = JSON.parse(json_array[i]);
            if (data.id == id) {
                item_total_price = parseFloat(data.price);
                break;
            }
        }
    }
    return item_total_price;
}
var Total_Cart_Item = function () {
    var total_qty = 0;
    var cart_records = Get_Storage_Record(session_variable_name);
    if (cart_records != null) {
        var json_array = [];
        json_array = JSON.parse(cart_records);
        for (var i = 0; i < json_array.length; i++) {
            var data = JSON.parse(json_array[i]);
            total_qty += parseInt(data.qty);
        }
    }
    return total_qty;
}
var Current_Item_Total = function (id) {
    var item_total_qty = 0;
    var cart_records = Get_Storage_Record(session_variable_name);
    if (cart_records != null) {
        var json_array = [];
        json_array = JSON.parse(cart_records);
        for (var i = 0; i < json_array.length; i++) {
            var data = JSON.parse(json_array[i]);
            if (data.id == id) {
                item_total_qty = parseInt(data.qty);
                break;
            }
        }
    }
    return item_total_qty;
}
var Add_Cart_Item = function (id, title, price, image_src, item_url, qty) {
    var json_array = [];
    var cart_records = Get_Storage_Record(session_variable_name);
    title = encodeURIComponent(title);
    image_src = encodeURIComponent(image_src);
    item_url = encodeURIComponent(item_url);
    var current_data = { "id": id, "title": title, "price": price, "image_src": image_src, "item_url": item_url, "qty": qty, "gifts" : []};
    if (cart_records == null) {
        json_array.push(JSON.stringify(current_data));
        Add_Storage_Record(session_variable_name, JSON.stringify(json_array));
        Add_Storage_Record(session_expire_time_variable_name, new Date());
    } else {
        json_array = JSON.parse(cart_records);
        var is_found = false;
        for (var i = 0; i < json_array.length; i++) {
            var data = JSON.parse(json_array[i]);
            if (data.id == id) {
                current_data.qty = (parseInt(data.qty) + parseInt(qty));
                current_data.gifts = data.gifts;
                json_array[i] = JSON.stringify(current_data);
                is_found = true;
                break;
            }
        }
        if (is_found == false) {
            json_array.push(JSON.stringify(current_data));
        }
        Add_Storage_Record(session_variable_name, JSON.stringify(json_array));
    }
    Update_Cart_Section();
}
var Update_Cart_Item = function (id, qty) {
    var cart_records = Get_Storage_Record(session_variable_name);
    var json_array = JSON.parse(cart_records);
    for (var i = 0; i < json_array.length; i++) {
        var data = JSON.parse(json_array[i]);
        if (data.id == id) {
            var current_data = { "id": data.id, "title": data.title, "price": data.price, "image_src": data.image_src, "item_url": data.item_url, "qty": qty, "gifts": [] };
            current_data.gifts = data.gifts;
            json_array[i] = JSON.stringify(current_data);
            break;
        }
    }
    Add_Storage_Record(session_variable_name, JSON.stringify(json_array));
    Update_Cart_Section();
}
var Remove_Cart_Item = function (id, type) {
    var json_array = [];
    var cart_records = Get_Storage_Record(session_variable_name);
    if (cart_records != null) {
        json_array = JSON.parse(cart_records);
        for (var i = 0; i < json_array.length; i++) {
            var data = JSON.parse(json_array[i]);
            if (data.id == id) {
                if (type == "W") {
                    data.qty = 0;
                } else {
                    data.qty -= 1;
                }
                if (data.qty == 0) {
                    json_array.splice(i, 1);
                } else {
                    json_array[i] = JSON.stringify(data);
                }
                break;
            }
        }
        if (json_array.length > 0) {
            Add_Storage_Record(session_variable_name, JSON.stringify(json_array));
        } else {
            ClearAllStorage();
            if (Current_Page_URL() == checkout_page_url) {
                window.location = web_url;
            }
        }    
    }
    Update_Cart_Section();
}
var Add_Cart_Gift_Item = function (item_id, id, title, price, image_src, item_url, qty) {
    var json_array = [];
    var cart_records = Get_Storage_Record(session_variable_name);
    title = encodeURIComponent(title);
    image_src = encodeURIComponent(image_src);
    item_url = encodeURIComponent(item_url);
    var current_data = { "id": id, "title": title, "price": price, "image_src": image_src, "item_url": item_url, "qty": qty };
    if (cart_records != null) {
        json_array = JSON.parse(cart_records);
        for (var i = 0; i < json_array.length; i++) {
            var data = JSON.parse(json_array[i]);
            if (data.id == item_id) {
                var gift_array = data.gifts;
                var is_found = false;
                for (var k = 0; k < gift_array.length; k++) {
                    var data1 = gift_array[k];
                    if (data1.id == id) {
                        current_data.qty = (parseInt(data1.qty) + parseInt(qty));
                        gift_array[k] = current_data;
                        is_found = true;
                        break;
                    }
                }
                if (is_found == false) {
                    gift_array.push(current_data);
                }
                data.gifts = gift_array;
                json_array[i] = JSON.stringify(data);
            }
        }
        Add_Storage_Record(session_variable_name, JSON.stringify(json_array));
    } 
}
var Remove_Cart_Gift_Item = function (item_id, id, type) {
    var json_array = [];
    var cart_records = Get_Storage_Record(session_variable_name);
    if (cart_records != null) {
        json_array = JSON.parse(cart_records);
        for (var i = 0; i < json_array.length; i++) {
            var data = JSON.parse(json_array[i]);
            if (data.id == item_id) {
                var gift_array = data.gifts;
                for (var k = 0; k < gift_array.length; k++) {
                    var data1 = gift_array[k];
                    if (data1.id == id) {
                        if (type == "W") {
                            data1.qty = 0;
                        } else {
                            data1.qty -= 1;
                        }
                        if (data1.qty == 0) {
                            gift_array.splice(k, 1);
                        } else {
                            gift_array[i] = data1;
                        }
                        break;
                    }
                }
                if (gift_array.length > 0) {
                    data.gifts = gift_array;
                } else {
                    data.gifts = [];
                }
                json_array[i] = JSON.stringify(data);  
            }
        }
        Add_Storage_Record(session_variable_name, JSON.stringify(json_array));
    }
    Update_Cart_Section();
}
$(document).on("click", ".add-to-cart", function () {
    var current_context = $(this);
    var qty = 1;
    if (current_context.hasClass("buy-now")) {
        qty = $("#cart_quantity").val();
        $(".gift-checkbox").each(function () {
            $(this).prop("checked", false);
        });
        $("#giftModal").modal("show");
    }
    else if (current_context.hasClass("quick")) {
        qty = $("#cart_quantity").val();
    }
    Add_Cart_Item(current_context.attr("data-id"), current_context.attr("data-title"), current_context.attr("data-price"), current_context.attr("data-image-url") + current_context.attr("data-image-src"), current_context.attr("data-url"), qty);
    if (current_context.parent().hasClass("quickview-btn-cart")) {
        current_context.notify(
            "Item Added to Cart",
            { position: "bottom", className: "success" }
        );
    } else {
        current_context.parents(".product-wrapper").find(".product-img").notify(
            "Item Added to Cart",
            { position: "bottom", className: "success" }
        );
    }
    
    return false;
});
$(document).on("click", ".add-to-gift", function () {
    var count = 0;
    $(".gift-checkbox").each(function () {
        var current_context = $(this);
        if (current_context.prop("checked") == true) {
            count += 1;
            Add_Cart_Gift_Item($("#item_hidden_id").val(), current_context.attr("data-id"), current_context.attr("data-title"), current_context.attr("data-price"), current_context.attr("data-image-src"), current_context.attr("data-url"), 1);
        }
    });
    $("#giftModal").modal("hide");
    if (count > 0) {
        $("#quickview-btn-cart").notify(
            "Item With Gift Added to cart",
            { position: "bottom", className: "success" }
        );
    }
    Update_Cart_Section();
    return false;
});
$(document).on("click", ".remove-gift-item", function () {
    var current_context = $(this);
    Remove_Cart_Gift_Item(current_context.attr("data-item-id"), current_context.attr("data-id"), current_context.attr("data-type"));
    return false;
});
$(document).on("click", ".remove-item", function () {
    var current_context = $(this);
    Remove_Cart_Item(current_context.attr("data-id"), current_context.attr("data-type"));
    return false;
});
function ShowWishListNotification(current_context, msg, className) {
    if (current_context.parent().hasClass("quickview-btn-wishlist")) {
        current_context.notify(
            msg,
            { position: "bottom", className: className }
        );
    } else {
        current_context.parents(".product-wrapper").find(".product-img").notify(
            msg,
            { position: "bottom", className: className }
        );
    }
}
$(document).on("click", ".add-to-wish-list", function () {
    var current_context = $(this);
    current_context.attr("disabled", "disabled");
    if ($("#wish_list_total_count").attr("data-login") == "Yes") {
        $.ajax({
            type: "Post",
            url: web_url + "wishlist",
            data: "id=" + current_context.attr("data-id"),
            success: function (response) {
                if (response.Success == true) {
                    ShowWishListNotification(current_context, response.Message, "success");
                    $("#wish_list_total_count").html(response.Data);
                } else {
                    ShowWishListNotification(current_context, response.Message, "error");
                }
                current_context.removeAttr("disabled");
            },
            error: function (data) {
                console.log(data);
                current_context.removeAttr("disabled");
            }
        });
    } else {
        ShowWishListNotification(current_context, "Please login to add item in wishlist", "error");
        current_context.removeAttr("disabled");
    }
    return false;
});
$(document).on("click", ".remove-wish-list-item", function () {
    var current_context = $(this);
    current_context.find("i").removeClass();
    current_context.find("i").addClass("ti-reload");
    current_context.attr("disabled", "disabled");
    $.ajax({
        type: "Post",
        url: web_url + "wishlist-remove",
        data: "id=" + current_context.attr("data-id"),
        success: function (response) {
            if (response.Success == true) {
                current_context.parents(".wish-list-row").remove();
                $("#wish_list_total_count").html(response.Data);
                if ($("#wish_list_table_body").find(".wish-list-row").length == 0) {
                    $('<tr>').addClass("text-danger text-center").html("<td colspan='4'>No Items in wishlist</td>").appendTo($("#wish_list_table_body"));
                }
            } else {
                current_context.notify(
                    response.Message,
                    { position: "bottom", className: "error" }
                );
            }
            current_context.find("i").removeClass();
            current_context.find("i").addClass("ion-ios-trash-outline");
            current_context.removeAttr("disabled");
        },
        error: function (data) {
            console.log(data);
            current_context.find("i").removeClass();
            current_context.find("i").addClass("ion-ios-trash-outline");
            current_context.removeAttr("disabled");
        }
    });
    return false;
});
/*--- showlogin toggle function ----*/
$(document).on('click', '#showcoupon', function () {
    $('#checkout_coupon').slideToggle(900);
});
$(document).on("click", ".remove-coupon", function () {
    Clear_Storage_Record(session_discount_variable_name);
    Update_Cart_Section();
    return false;
});
$(document).on("click", "#btn_apply_coupon", function () {
    $(".coupon-error").remove();
    var current_context = $(this);
    if ($("#coupon_input_code").val() == "") {
        $('<span>').addClass("text-danger coupon-error").html("Required").insertAfter($("#coupon_input_code").parent());
    } else {
        current_context.html("<i class='ti-reload'></i> Apply Coupon");
        $.ajax({
            type: "Post",
            url: $("#coupon_area").attr("data-url"),
            data: "code=" + $("#coupon_input_code").val(),
            success: function (response) {
                if (response.Success == true) {
                    $("#coupon_area").html("");
                    Add_Storage_Record(session_discount_variable_name, JSON.stringify(response.Data));
                    Update_Cart_Section();
                } else {
                    $('<span>').addClass("text-danger coupon-error").html(response.Message).insertAfter($("#coupon_input_code").parent());
                    current_context.html("Apply Coupon");
                }
            },
            error: function (data) {
                console.log(data);
                current_context.html("Apply Coupon");
            }
        });
    }
});
var Update_Cart_Section = function () {
    var cart_listing_area_context = $("#cart_listing_area");
    cart_listing_area_context.html("");
    var cart_table_listing_area_context = $("#cart_table_listing_area");
    if (cart_table_listing_area_context.length > 0) {
        cart_table_listing_area_context.html("");
    }
    var cart_total_table_listing_area_context = $("#cart_total_table_listing_area");
    if (cart_total_table_listing_area_context.length > 0) {
        cart_total_table_listing_area_context.html("");
    }
    var checkout_table_listing_area_context = $("#checkout_table_listing_area");
    if (checkout_table_listing_area_context.length > 0) {
        checkout_table_listing_area_context.html("");
    }
    var total_cart_item = Total_Cart_Item();
    $("#cart_total_count").html(total_cart_item);
    if (total_cart_item > 0) {
        var session_discount_data = Get_Storage_Record(session_discount_variable_name);
        cart_total_table_listing_area_context.parent().parent().show();
        var json_array = [];
        json_array = JSON.parse(Get_Storage_Record(session_variable_name));
        for (var i = 0; i < json_array.length; i++) {
            var record = JSON.parse(json_array[i]);
            var item_area = $("<li>").addClass("single-product-cart").appendTo(cart_listing_area_context);
            var item_img_area = $("<div>").addClass("cart-img").appendTo(item_area);
            var item_img_anchor = $("<a>").attr("href", decodeURIComponent(record.item_url)).appendTo(item_img_area);
            $("<img>").attr("src", decodeURIComponent(record.image_src)).attr("style", "width:100px").appendTo(item_img_anchor);
            var item_title_area = $("<div>").addClass("cart-title").appendTo(item_area);
            $("<h3>").html("<a href='" + decodeURIComponent(record.item_url) + "'> " + decodeURIComponent(record.title) + "</a>").appendTo(item_title_area);
            $("<span>").html(record.qty + " x " + web_currency + " " + parseFloat(record.price).toFixed(2)).appendTo(item_title_area);
            $("<span>").html("Total : " + web_currency + " " + parseFloat(record.qty * record.price).toFixed(2)).appendTo(item_title_area);
            if (record.gifts.length > 0) {
                var gift_area = $('<div>').addClass("gift-area").appendTo(item_title_area);
                $('<hr>').appendTo(gift_area);
                $('<h4>').html("Addon Products").appendTo(gift_area);
                for (var k = 0; k < record.gifts.length; k++) {
                    var record1 = record.gifts[k];
                    $("<h5>").html("<a href='#'>--- " + decodeURIComponent(record1.title) + "</a>").appendTo(gift_area);
                    $("<span>").html(record1.qty + " x " + web_currency + " " + parseFloat(record1.price).toFixed(2)).appendTo(gift_area);
                    $("<span>").html("Total : " + web_currency + " " + parseFloat(record1.qty * record1.price).toFixed(2)).appendTo(gift_area);
                    var gift_delete_area = $("<div>").addClass("gift-delete").appendTo(gift_area);
                    $("<a>").attr("href", "#").addClass("remove-gift-item").attr("data-item-id", record.id).attr("data-id", record1.id).attr("data-type", "S").html("<i class='ti-trash'></i>").appendTo(gift_delete_area);
                    $('<hr>').appendTo(gift_area);
                }
            }
            var item_delete_area = $("<div>").addClass("cart-delete").appendTo(item_area);
            $("<a>").attr("href", "#").addClass("remove-item").attr("data-id", record.id).attr("data-type", "S").html("<i class='ti-trash'></i>").appendTo(item_delete_area);
            //Cart Table
            if (cart_table_listing_area_context.length > 0) {
                var table_row = $('<tr>').appendTo(cart_table_listing_area_context);
                var item_table_img_area = $('<td>').addClass("product-thumbnail").appendTo(table_row);
                var item_table_img_anchor = $("<a>").attr("href", decodeURIComponent(record.item_url)).appendTo(item_table_img_area);
                $("<img>").attr("src", decodeURIComponent(record.image_src)).attr("style", "width:100px").appendTo(item_table_img_anchor);
                var name_column_area = $('<td>').addClass("product-name").html("<a href='" + decodeURIComponent(record.item_url) + "'>" + decodeURIComponent(record.title) + "</a>").appendTo(table_row);
                if (record.gifts.length > 0) {
                    var gift_area = $('<div>').addClass("gift-area").appendTo(name_column_area);
                    $('<hr>').appendTo(gift_area);
                    $('<h4>').html("Addon Products").appendTo(gift_area);
                    for (var k = 0; k < record.gifts.length; k++) {
                        var record1 = record.gifts[k];
                        $("<img>").attr("src", decodeURIComponent(record1.image_src)).attr("style", "width:50px").appendTo(gift_area);
                        $("<h5>").html("<a href='#'>" + decodeURIComponent(record1.title) + "</a>").appendTo(gift_area);
                        $("<span>").html(record1.qty + " x " + web_currency + " " + parseFloat(record1.price).toFixed(2)).appendTo(gift_area);
                        $("<span>").html("Total : " + web_currency + " " + parseFloat(record1.qty * record1.price).toFixed(2)).appendTo(gift_area);
                        var gift_delete_area = $("<div>").addClass("gift-delete").appendTo(gift_area);
                        $("<a>").attr("href", "#").addClass("remove-gift-item").attr("data-item-id", record.id).attr("data-id", record1.id).attr("data-type", "S").html("<i class='ti-trash'></i>").appendTo(gift_delete_area);
                        $('<hr>').appendTo(gift_area);
                    }
                }
                var item_table_quantity_area = $("<td>").addClass("product-quantity").appendTo(table_row);
                $("<div>").addClass("quantity-range").html('<input class="input-text qty text cart-quantity" data-id="' + record.id + '" type="number" step="1" min="1" value="' + record.qty + '" title="Qty" size="4">').appendTo(item_table_quantity_area);
                $('<td>').addClass("product-price").html("<span class='amount'>" + web_currency + " " + parseFloat(record.price).toFixed(2) + "</span>").appendTo(table_row);
                $('<td>').addClass("product-subtotal").html("<span class='amount'>" + web_currency + " " + parseFloat(record.price * record.qty).toFixed(2) + "</span>").appendTo(table_row);
                var item_table_delete_area = $("<td>").addClass("product-cart-icon product-subtotal").appendTo(table_row);
                $("<a>").attr("href", "#").addClass("remove-item").attr("data-id", record.id).attr("data-type", "S").html('<i class="ion-ios-trash-outline" aria-hidden="true"></i>').appendTo(item_table_delete_area);
            }
            //Checkout Table
            if (checkout_table_listing_area_context.length > 0) {
                var coupon_area = $("#coupon_area");
                coupon_area.html("");
                if (session_discount_data == null) {
                    $('<h3>').html('Have a coupon? <span id="showcoupon">Click here to enter your code</span>').appendTo(coupon_area);
                    var coupon_checkout_area = $('<div>').addClass("coupon-checkout-content").attr("id", "checkout_coupon").appendTo(coupon_area);
                    var coupon_info_area = $('<div>').addClass("coupon-info").appendTo(coupon_checkout_area);
                    var coupon_para_area = $('<p>').addClass("checkout-coupon").appendTo(coupon_info_area);
                    $('<input>').attr("id", "coupon_input_code").attr("type", "text").attr("placeholder", "Coupon Code").appendTo(coupon_para_area);
                    $('<button>').attr("type", "button").attr("id", "btn_apply_coupon").html("Apply Coupon").appendTo(coupon_para_area);
                }
                var table_checkout_row = $('<tr>').addClass("cart_item").appendTo(checkout_table_listing_area_context);
                var item_table_checkout_area = $('<td>').addClass("product-name").html(decodeURIComponent(record.title)).appendTo(table_checkout_row);
                $('<strong>').html(" × " + record.qty).addClass("product-quantity").appendTo(item_table_checkout_area);
                if (record.gifts.length > 0) {
                    var gift_area = $('<div>').addClass("gift-area").appendTo(item_table_checkout_area);
                    $('<hr>').appendTo(gift_area);
                    $('<h4>').html("Addon Products").appendTo(gift_area);
                    for (var k = 0; k < record.gifts.length; k++) {
                        var record1 = record.gifts[k];
                        $("<h5>").html("<a href='#'>" + decodeURIComponent(record1.title) + "</a>").appendTo(gift_area);
                        $("<span>").html(record1.qty + " x " + web_currency + " " + parseFloat(record1.price).toFixed(2)).appendTo(gift_area);
                    }
                }
                var item_price_table_checkout_area = $('<td>').addClass("product-total").appendTo(table_checkout_row);
                $('<span>').html(web_currency + " " +  parseFloat(record.price * record.qty).toFixed(2)).addClass("amount").appendTo(item_price_table_checkout_area);
            }
        }
        var total_area = $("<li>").addClass("single-product-cart").appendTo(cart_listing_area_context);
        var total_inner_area = $("<div>").addClass("cart-total").appendTo(total_area);
        sub_total = Total_Cart_Price();
        discount_total = parseFloat(0);
        grand_total = parseFloat(0);
        tax_amount = parseFloat(0); 
        $("<h4>").html("Sub Total : <span>" + web_currency + " " + sub_total.toFixed(2) + "</span>").appendTo(total_inner_area);
        if (session_discount_data != null) {
            var session_data_record = JSON.parse(session_discount_data);
            if (session_data_record.Type == "Percent") {
                discount_total = parseFloat(sub_total / 100 * session_data_record.Amount);
            } else {
                discount_total = parseFloat(session_data_record.Amount);
            }
            $("<h4>").html("Discount Total (<a href='#' class='remove-coupon' title='remove'><i class='ti-trash'></i></a>) : <span>" + web_currency + " " + discount_total.toFixed(2) + "</span>").appendTo(total_inner_area);
        }
        if (website_delivery_charges > 0) {
            $("<h4>").html("Delivery Charges : <span>" + web_currency + " " + website_delivery_charges.toFixed(2) + "</span>").appendTo(total_inner_area);
        }
        if (website_tax_percentage > 0) {
            tax_amount = parseFloat((sub_total - discount_total + website_delivery_charges) / 100 * website_tax_percentage);
            $("<h4>").html("Tax (" + parseFloat(website_tax_percentage).toFixed(2) + "%) : <span>" + web_currency + " " + tax_amount.toFixed(2) + "</span>").appendTo(total_inner_area);
        }
        grand_total = parseFloat(sub_total - discount_total + website_delivery_charges + tax_amount);
        $("<h4>").html("Grand Total : <span>" + web_currency + " " + grand_total.toFixed(2) + "</span>").appendTo(total_inner_area);
        if ($(".cart-checkout-btn").length == 0) {
            var button_area = $('<div>').addClass("cart-checkout-btn").insertAfter(cart_listing_area_context);
            $("<a>").addClass("cr-btn btn-style cart-btn-style bg-cart").attr("href", web_url + cart_page_url).html("<span>view cart</span>").appendTo(button_area);
            $("<a>").addClass("no-mrg cr-btn btn-style cart-btn-style").attr("href", web_url + checkout_page_url).html("<span>checkout</span>").appendTo(button_area);
        }
        if (cart_total_table_listing_area_context.length > 0) {
            $("<li>").html("Sub Total : <span>" + web_currency + " " + sub_total.toFixed(2) + "</span>").appendTo(cart_total_table_listing_area_context);
            if (discount_total > 0) {
                $("<li>").html("Discount Total (<a href='#' class='remove-coupon' title='remove'><i class='ti-trash'></i></a>) : <span>" + web_currency + " " + discount_total.toFixed(2) + "</span>").appendTo(cart_total_table_listing_area_context);
            }
            if (website_delivery_charges > 0) {
                $("<li>").html("Delivery Charges : <span>" + web_currency + " " + website_delivery_charges.toFixed(2) + "</span>").appendTo(cart_total_table_listing_area_context);
            }
            if (website_tax_percentage > 0) {
                $("<li>").html("Tax (" + parseFloat(website_tax_percentage).toFixed(2) + "%) : <span>" + web_currency + " " + tax_amount.toFixed(2) + "</span>").appendTo(cart_total_table_listing_area_context);
            }
            $("<li>").html("Grand Total : <span>" + web_currency + " " + grand_total.toFixed(2) + "</span>").appendTo(cart_total_table_listing_area_context);
        }
        if (checkout_table_listing_area_context.length > 0) {
            var table_sub_checkout_row = $('<tr>').addClass("cart_item").appendTo(checkout_table_listing_area_context);
            $('<th>').addClass("cart-subtotal").html("Sub Total").appendTo(table_sub_checkout_row);
            var item_sub_price_table_checkout_area = $('<td>').appendTo(table_sub_checkout_row);
            $('<span>').html(web_currency + " " + sub_total.toFixed(2)).addClass("amount").appendTo(item_sub_price_table_checkout_area);
            if (discount_total > 0) {
                var table_discount_checkout_row = $('<tr>').addClass("cart_item").appendTo(checkout_table_listing_area_context);
                $('<th>').addClass("cart-subtotal").html("Discount Total (<a href='#' class='remove-coupon' title='remove'><i class='ti-trash'></i></a>)").appendTo(table_discount_checkout_row);
                var item_discount_price_table_checkout_area = $('<td>').appendTo(table_discount_checkout_row);
                $('<span>').html(web_currency + " " + discount_total.toFixed(2)).addClass("amount").appendTo(item_discount_price_table_checkout_area);
            }
            if (website_delivery_charges > 0) {
                var table_delivery_checkout_row = $('<tr>').addClass("cart_item").appendTo(checkout_table_listing_area_context);
                $('<th>').addClass("cart-subtotal").html("Delivery Charges").appendTo(table_delivery_checkout_row);
                var item_delivery_price_table_checkout_area = $('<td>').appendTo(table_delivery_checkout_row);
                $('<span>').html(web_currency + " " + website_delivery_charges.toFixed(2)).addClass("amount").appendTo(item_delivery_price_table_checkout_area);
            }
            if (website_tax_percentage > 0) {
                var table_tax_checkout_row = $('<tr>').addClass("cart_item").appendTo(checkout_table_listing_area_context);
                $('<th>').addClass("cart-subtotal").html("Tax (" + parseFloat(website_tax_percentage).toFixed(2) + ")").appendTo(table_tax_checkout_row);
                var item_tax_price_table_checkout_area = $('<td>').appendTo(table_tax_checkout_row);
                $('<span>').html(web_currency + " " + tax_amount.toFixed(2)).addClass("amount").appendTo(item_tax_price_table_checkout_area);
            }
            var table_grand_checkout_row = $('<tr>').addClass("cart_item").appendTo(checkout_table_listing_area_context);
            $('<th>').addClass("cart-subtotal").html("Grand Total").appendTo(table_grand_checkout_row);
            var item_grand_price_table_checkout_area = $('<td>').appendTo(table_grand_checkout_row);
            $('<span>').html(web_currency + " " + grand_total.toFixed(2)).addClass("amount").appendTo(item_grand_price_table_checkout_area);
            if (website_minimum_amount > 0) {
                var table_grand_checkout_row = $('<tr>').addClass("cart_item").appendTo(checkout_table_listing_area_context);
                $('<th>').attr("colspan", "2").addClass("cart-subtotal").html("<strong>Note : </strong> Minimum Order Amount is " + web_currency + " " + website_minimum_amount.toFixed(2)).appendTo(table_grand_checkout_row);
            }
        }
    } else {
        $(".cart-checkout-btn").remove();
        $('<li>').addClass("text-danger text-center").html("No Items in Cart").appendTo(cart_listing_area_context);
        if (cart_table_listing_area_context.length > 0) {
            $('<tr>').addClass("text-danger text-center").html("<td colspan='6'>No Items in Cart</td>").appendTo(cart_table_listing_area_context);
        }
        cart_total_table_listing_area_context.parent().parent().hide();
    }
}
$(document).on("change", ".cart-quantity", function () {
    var current_context = $(this);
    Update_Cart_Item(current_context.attr("data-id"), current_context.val());
});
$(document).on('click', '.qtybutton', function () {
    var $button = $(this);
    var oldValue = $button.parent().find('input').val();
    if ($button.text() === "+") {
        var newVal = parseFloat(oldValue) + 1;
    } else {
        // Don't allow decrementing below zero
        if (oldValue > 1) {
            var newVal = parseFloat(oldValue) - 1;
        } else {
            newVal = 1;
        }
    }
    $button.parent().find('input').val(newVal);
});
//Quick View Start
var isOpen = false;
$(document).on("click", ".open-quick-view", function () {
    var current_context = $(this);
    if (!isOpen) {
        current_context.find("i").removeClass().addClass("ti-reload icon-spin");
        isOpen = true;
        $.ajax({
            type: "Post",
            url: current_context.attr("href"),
            data: "id=" + current_context.attr("data-id"),
            success: function (response) {
                if (response.Success == true) {
                    var divArea = $(response.Data);
                    divArea.modal("show");
                    divArea.find('.testimonial-active').owlCarousel({
                        loop: true,
                        autoplay: false,
                        autoplayTimeout: 5000,
                        navText: ['<i class="ti-angle-left"></i>', '<i class="ti-angle-right"></i>'],
                        nav: true,
                        item: 1,
                        responsive: {
                            0: {
                                items: 1
                            },
                            768: {
                                items: 1
                            },
                            1000: {
                                items: 1
                            }
                        }
                    });
                    var CartPlusMinus = divArea.find('.cart-plus-minus');

                    CartPlusMinus.prepend('<div class="dec qtybutton">-</div>');
                    CartPlusMinus.append('<div class="inc qtybutton">+</div>');
                    $('.lazy').lazy();
                }
                current_context.find("i").removeClass().addClass("ti-plus");

            },
            error: function (data) {
                console.log(data);
                current_context.find("i").removeClass().addClass("ti-plus");
                isOpen = false;
            }
        });
    }
    return false;
});
$(document).on("hidden.bs.modal", "#quickModal", function () {
    isOpen = false;
    $("#quickModal").remove();
});
//Quick View End
var DisableView = function () {
    $('form').find("input[type='text'],input[type='password'],textarea,input[type='checkbox'],input[type='radio'],select").each(function () {
        $(this).attr("disabled", "disabled");
    });
    $('form').find("button").remove();
    $('form').removeAttr("action");
}
var EnableDisableArea = function (area, object_type) {
    var main_context = $('form');
    if (area != "") {
        main_context = $(area);
    }
    $(".waiting-area").remove();
    if (object_type == "Disable") {
        $(".button-submit").html($(".button-submit").attr("data-text") + " <i class='ti-reload icon-spin'></i>");
        $('<div>').addClass("waiting-area").html("<img src='/assets/images/loading.gif' alt='' /> <span style='color:#ffcc00'>submitting ....</span>").insertAfter($(".button-submit"));
        main_context.find("submit,button,input[type='checkbox'],input[type='radio'],select").each(function () {
            $(this).attr("disabled", "disabled");
        });
        main_context.find("input[type='text'],input[type='password'],textarea").each(function () {
            $(this).attr("readonly", "readonly");
        });
        main_context.find("a").each(function () {
            $(this).addClass("disabled");
        });
    } else {
        main_context.find("input[type='text'],input[type='password'],textarea").each(function () {
            $(this).removeAttr("readonly");
        });
        main_context.find("a").each(function () {
            $(this).removeClass("disabled");
        });
        main_context.find("submit,button,input[type='checkbox'],input[type='radio'],select").each(function () {
            $(this).removeAttr("disabled");
        });
        $(".button-submit").html($(".button-submit").attr("data-text"));
    }
}
OnFormBegin = function () {
    EnableDisableArea("", "Disable");
}
OnFormComplete = function () {

}
OnFormFailure = function (response) {
    EnableDisableArea("", "Enable");
    console.log(response);
}
OnFormSuccess = function (response) {
    console.log(response);
    var responseTypeArray = response.Type.split('-');
    $.each(responseTypeArray, function (key, value) {
        if (value == "M") {
            if (response.Success == true) {
                if ($(".msg-area").length > 0) {
                    $(".msg-area").notify(
                        response.Message,
                        { position: "bottom", className: "success" }
                    );
                } else {
                    $.notify(response.Message, "success");
                }
            } else {
                if ($(".msg-area").length > 0) {
                    $(".msg-area").notify(
                        response.Message,
                        { position: "bottom", className: "error" }
                    );
                } else {
                    $.notify(response.Message, "error");
                }
            }
        } else if (value == "F") {
            if (response.Success == true) {
                $("#" + response.FieldName).notify(
                    response.Message,
                    { position: "bottom", className: "success" }
                );
            } else {
                $("#" + response.FieldName).notify(
                    response.Message,
                    { position: "bottom", className: "error" }
                );
            }
        } else if (value == "R") {
            $('form')[0].reset();
        } else if (value == "T") {
            window.location = response.TargetURL;
        } else if (value == "TD") {
            setTimeout(function () {
                window.location = response.TargetURL;
            }, 2000);
        } else if (value == "RL") {
            window.location.reload();
        } else if (value == "RLD") {
            setTimeout(function () {
                window.location.reload();
            }, 2000);
        }
    });
    EnableDisableArea("", "Enable");
}
var min_price = 0;
var max_price = 0;
var page_number = 1;
var LoadShopData = function (is_filter) {
    var shop_area_context = $("#shop_area");
    if (is_filter) {
        shop_area_context.find(".row").html("");
    }
    var loader_area = $('<div>').addClass("text-center").html("<img src='/assets/images/loading.gif' alt='' />").appendTo(shop_area_context);
   // scrollToPosition(loader_area);
    var sortByArray = $("#sort_by").val().split("|");
    var sort_by = sortByArray[0];
    var dir = sortByArray[1];
    var categories = "";
    $(".shop-category").each(function () {
        if ($(this).prop("checked") == true) {
            categories += $(this).val() + ",";
        }
    });
    $.ajax({
        type: "Post",
        url: $("#shop_area").attr("data-href"),
        data: "sort_by=" + sort_by + "&dir=" + dir + "&min_price=" + min_price + "&max_price=" + max_price + "&page=" + page_number + "&categories=" + categories + "&search_text=" + $("#search_text").val(),
        success: function (response) {
            if (response.Success == true) {
                $(response.Data).appendTo(shop_area_context.find(".row"));
                $('.lazy').lazy();
                if (response.FieldName != "") {
                    if ($(".btn-load-more").length == 0) {
                        var load_row_area = $('<div>').addClass("row load-more").insertAfter(shop_area_context);
                        var load_row_inner_area = $('<div>').addClass("col-md-12 text-center").appendTo(load_row_area);
                        $('<a>').attr("href", "#").attr("data-page-number", response.FieldName).addClass("btn-style mt-10 btn-load-more").html("Load More").appendTo(load_row_inner_area);
                    } else {
                        $(".btn-load-more").attr("data-page-number", response.FieldName);
                    }
                } else {
                    $(".btn-load-more").parents(".load-more").remove();
                }
            } else {
                $(".btn-load-more").parents(".load-more").remove();
            }
            loader_area.remove();
        },
        error: function (data) {
            console.log(data);
            loader_area.remove();
        }
    });
}
//$(document).ready(function () {
//    debugger;
//    page_number = 1;
//    $("#shop_area").find(".row").html("");
//    $(".load-more").remove();
//    LoadShopData(true);
//    return false;    
//});
$(document).on("change", "#sort_by", function () {
    page_number = 1;
    $("#shop_area").find(".row").html("");
    $(".load-more").remove();
    LoadShopData(true);
    return false;
});
$(document).on("change", ".shop-category", function () {
    page_number = 1;
    $("#shop_area").find(".row").html("");
    $(".load-more").remove();
    LoadShopData(true);
});
$(document).on("click", ".btn-load-more", function () {
    page_number = $(this).attr("data-page-number");
    LoadShopData(false);
    return false;
});
$(document).on("keyup", "#search_text", function () {
    delay(function () {
        LoadShopData(true);
    }, 2000);
});
$(document).on("click", "#btn_checkout", function () {
    var current_context = $(this);
    if ($("#FormCheckOut").valid()) {
        if (website_minimum_amount > grand_total) {
            $("#btn_checkout").notify(
                "Minimum Order Amount is " + web_currency + website_minimum_amount.toFixed(2) + " Please add more products",
                { position: "bottom", className: "error" }
            );
        } else {
            current_context.html("Place Order <i class='ti-reload'></i>");
            current_context.attr("disabled", "disabled");
            var session_data = Get_Storage_Record(session_variable_name);
            var session_discount_data = Get_Storage_Record(session_discount_variable_name);
            var serialize_form = $("#FormCheckOut").serialize();
            $.ajax({
                type: "Post",
                url: $("#FormCheckout").attr("action"),
                data: "SessionData=" + session_data + "&SubTotal=" + sub_total + "&DiscountTotal=" + discount_total + "&DeliveryCharges=" + website_delivery_charges + "&Tax=" + tax_amount + "&GrandTotal=" + grand_total + "&DiscountData=" + session_discount_data + "&form_data=" + serialize_form,
                success: function (response) {
                    if (response.Success == true) {
                        ClearAllStorage();
                    }
                    OnFormSuccess(response);
                },
                error: function (data) {
                    console.log(data);
                    $("#btn_checkout").notify(
                        "some thing went wrong please try again later",
                        { position: "bottom", className: "error" }
                    );
                    current_context.html("Place Order");
                    current_context.removeAttr("disabled");
                }
            });
        }      
    }
});
$(document).on("change", ".rating-selection", function () {
    $("#Star").val($(this).val());
});
var LoadPriceSlider = function () {
    if ($('#price-range').length > 0) {
        /*-- Price Range --*/
        var PriceRange = $('#price-range');
        var PriceAmount = $('.price-amount');
      
        PriceRange.slider({
            range: true,
            min: 0,
            max: PriceRange.attr("data-max"),
            values: [0, PriceRange.attr("data-max")],
            slide: function (event, ui) {
                PriceAmount.val(web_currency + ui.values[0] + ' - ' + web_currency + ui.values[1]);
                min_price = ui.values[0];
                max_price = ui.values[1];
                delay(function () {
                    page_number = 1;
                    LoadShopData(true);
                }, 2000);
            }
        });
        PriceAmount.val(web_currency + PriceRange.slider('values', 0) +
            ' - ' + web_currency + PriceRange.slider('values', 1));
        min_price = PriceRange.slider('values', 0);
        max_price = PriceRange.slider('values', 1);
    }
}
$(document).ready(function () {
    $.validator.setDefaults({
        focusInvalid: false
    });

    function scrollToError(error, validator) {
        var elem = $(validator.errorList[0].element);
      
        if (elem.length) {
            if (elem.is(':visible'))
                return elem.offset().top - 100;
            elem = elem.prev($(".select2-container"));
            if (elem.length) {
                return elem.offset().top - 100;
            }
        }
        return 0; // scroll to top if all else fails
    }

    $('form').bind('invalid-form.validate', function (error, validator) {
        // fix scrolling and validation for select2
       
        if (!validator.numberOfInvalids())
            return;

        $('html, body').animate({
            scrollTop: scrollToError(error, validator)
        }, 500);
    });
    $.ajax({
        type: "Post",
        url: "/get-setting",
        data: "id=0",
        success: function (response) {
            if (response.Success == true) {
                web_currency = response.Data.Currency;
                website_delivery_charges = parseFloat(response.Data.DeliveryCharges);
                website_tax_percentage = parseFloat(response.Data.TaxPercentage);
                website_minimum_amount = parseFloat(response.Data.MinimumOrderAmount);
                Update_Cart_Section();
                LoadPriceSlider();
            }
        },
        error: function (data) {
            console.log(data);
            Update_Cart_Section();
            LoadPriceSlider();
        }
    });
    if (Current_Page_URL() == checkout_page_url) {
        if (Total_Cart_Item() == 0) {
            window.location = web_url;
        } else {
        }
    }
    if ($(".date-picker").length > 0) {
        $(".date-picker").datepicker({
            minDate: new Date()
        });
    }
    if ($(".lazy").length > 0) {
        $('.lazy').lazy();
    } 
});
$(".owl-carousel1").owlCarousel({
    loop: true,
    center: true,
    margin: 0,
    responsiveClass: true,
    nav: false,
    responsive: {
        0: {
            items: 1,
            nav: false
        },
        100: {
            items: 1,
            nav: false,
            loop: false
        },
        500: {
            items: 3,
            nav: true
        }
    }
});
$(window).on("load", function (e) {
    $("#loader_wrapper").css({ "display": "none" });
});