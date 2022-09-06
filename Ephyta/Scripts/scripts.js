function IndexJs() {
    $(".banner-list").slick({
        dots: true,
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: false,
        autoplay: true,
    });
    $(".product-list").slick({
        dots: false,
        infinite: true,
        slidesToShow: 4,
        slidesToScroll: 4,
        arrows: true,
        autoplay: true,
        autoplaySpeed: 5000,
        prevArrow: '<div class="slick-prev"><i class="far fa-angle-left"></i></div>',
        nextArrow: '<div class="slick-next"><i class="far fa-angle-right"></i></div>',
        responsive: [
            {
                breakpoint: 1024,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 3
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 2
                }
            },
        ]
    });
    $(".new-slider").slick({
        infinite: true,
        slidesToShow: 2,
        slidesToScroll: 2,
        arrows: true,
        autoplay: true,
        prevArrow: '<div class="slick-prev"><i class="far fa-angle-left"></i></div>',
        nextArrow: '<div class="slick-next"><i class="far fa-angle-right"></i></div>',
        responsive: [
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
        ]
    });
    $(".feedback-list").slick({
        dots:true,
        infinite: true,
        slidesToShow: 3,
        slidesToScroll: 3,
        arrows: false,
        autoplay: true,
        responsive: [

            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                }
            },
        ]
        
    });
    $(".partner-list").slick({
        dots: false,
        infinite: true,
        slidesToShow: 6,
        slidesToScroll: 3,
        arrows: true,
        autoplay: true,
        prevArrow: '<div class="slick-prev"><i class="far fa-angle-left"></i></div>',
        nextArrow: '<div class="slick-next"><i class="far fa-angle-right"></i></div>',
    });
}
document.addEventListener('lazybeforeunveil', function (e) {
    var bg = e.target.getAttribute('data-bg');
    if (bg) {
        e.target.style.backgroundImage = 'url(' + bg + ')';
    }
});

$(function () {
    var ratings = document.getElementsByClassName('rating');
    for (var i = 0; i < ratings.length; i++) {
        var r = new SimpleStarRating(ratings[i]);

        ratings[i].addEventListener('rate', function (e) {
            console.log('Rating: ' + e.detail);
        });
    }

    $("#register_form").on("submit", function (e) {
        e.preventDefault();
        if ($(this).valid()) {
            $.post("/Home/FormRegister", $(this).serialize(), function (data) {
                if (data.status) {
                    $.toast({
                        heading: 'Liên hệ thành công',
                        text: data.msg,
                        icon: 'success'
                    })
                    $("#register_form").trigger("reset");
                } else {
                    $.toast({
                        heading: 'Liên hệ không thành công',
                        text: data.msg,
                        icon: 'error'
                    })
                }
            });
        }
    });

    $("#contact_form").on("submit",
        function(e) {
            e.preventDefault();
            $.post("/Home/Contact",
                $(this).serialize(),
                function(data) {
                    if (data.status) {
                        alert(data.msg);
                        $("#contact_form").trigger("reset");
                    } else {
                        alert(data.msg);
                    }
                });
        });

    var $inputItem = $(".js-inputWrapper");
    $inputItem.length &&
        $inputItem.each(function() {
            var $this = $(this),
                $input = $this.find(".formRow--input"),
                placeholderTxt = $input.attr("placeholder"),
                $placeholder;

            $input.after('<span class="placeholder">' + placeholderTxt + "</span>"),
                $input.attr("placeholder", ""),
                $placeholder = $this.find(".placeholder"),
                $input.val().length ? $this.addClass("active") : $this.removeClass("active"),
                $input.on("focusout",
                    function() {
                        $input.val().length ? $this.addClass("active") : $this.removeClass("active");
                    }).on("focus",
                    function() {
                        $this.addClass("active");
                    });
        });

   
    $(".category-child").mouseover(function () {
        var categoryId = $(this).attr("data-id");

        /// tô đậm

        $(".category-child").addClass("no-bold")
        $(this).removeClass("no-bold")

        $(".category-child").removeClass("bold")
        $(this).addClass("bold");


        /// Hiện product
        $(".product-menu ul").removeClass("no-active")

        $(".product-menu ul").addClass("no-active")


        $(".product-menu ul").removeClass("active")
        $("#" + categoryId).addClass("active");
        console.log(categoryId)
    });

    $(".menu-child").mouseleave (function () {
        
        $(".product-menu ul").removeClass("no-active")
        $(".product-menu ul").removeClass("active")

        $(".category-child").removeClass("no-bold")
        $(".category-child").removeClass("bold")

     
       
        console.log("mouseover")
    });

    $("[data-item=city]").on("change", function (data) {

      /*  console.log(data);*/

        const id = $(this).val();
        var items = [];
        items.push("<option value>Hãy chọn quận huyện</option>");

        if (id !== "") {
            $.getJSON("/Base/GetDistrict", { cityId: id }, function (data) {
                $.each(data, function (key, val) {
                    items.push("<option value='" + val.Id + "'>" + val.Name + "</option>");
                });
                $("[data-item=district]").html(items.join(""));
            });
        } else {
            $("[data-item=district]").html(items.join(""));
        }
    });
    if ($("[data-item=ward]").length) {
        $("[data-item=district]").on("change", function (data) {
            const id = $(this).val();
            var items = [];
            items.push("<option value>Hãy chọn Phường xã</option>");

            if (id !== "") {
                $.getJSON("/Base/GetWard", { districtId: id }, function (data) {
                    $.each(data, function (key, val) {
                        items.push("<option value='" + val.Id + "'>" + val.Name + "</option>");
                    });
                    $("[data-item=ward]").html(items.join(""));
                });
            } else {
                $("[data-item=ward]").html(items.join(""));
            }
        });
    }


    
    //$(".remove-product").click(function () {
        
    //});
    AOS.init();
});

function removeProduct(thisD) {
    if (confirm("Bạn có chắc chắn xóa sản phẩm này khỏi giỏ hàng?")) {
        const recordToDelete = $(thisD).attr("data-id");

        console.log(recordToDelete);
        if (recordToDelete !== "") {
            $.post("/ShoppingCart/RemoveFromCart", { "id": recordToDelete }, function (data) {
                if (data.Status === 1) {
                    $("tr[data-row='" + recordToDelete + "']").fadeOut();
                } else {
                    alert("Quá trình thực hiện không thành công");
                }
                window.location.reload();
            });
        }
    }
}

function showHideListMenuMobile(thisD) {
    let elemtC = $(thisD).parent().find('ul');
    let displayC = elemtC.css('display');
    if (displayC == 'none') {
        elemtC.css('display', 'block');
        $(thisD).html('<i class="fas fa-chevron-down"></i>');
    } else {
        elemtC.css('display', 'none');
        $(thisD).html('<i class="fas fa-chevron-right"></i>');
    }
}

function showMenuMobi() {
    $(".hamburger").toggleClass("is-active");
    $(".menu-mobi").toggleClass("active");
    $(".overlay-all").toggleClass("active");
}

//function closeMenuMobi() {
//    $(".hamburger").removeClass("is-active");
//    $(".overlay-all").removeClass("active");
//}

function scrollHeaderMenu(idD) {
    let valS = parseInt($('#' + idD).scrollLeft() + 150);
    $('#' + idD).animate({ scrollLeft: valS }, 150);
}

function addToCart(n,m) {
    $.post("/gio-hang/them-vao-gio-hang", { productId: n }, function (n) {

        if (n.result === 1) {
            $.toast({
                text: "Thêm vào giỏ hàng thành công",
                icon: "success",
                position: "top-center"
            });
            $("#itemshop").text(n.count);
            if (m === "checkout") {
                window.location.href = "/gio-hang/thong-tin";
            }
        } else {
            $.toast({
                text: "Quá trình thực hiện không thành công",
                icon: "error",
            });
        }

        //n.result === 1
        //    ? ($.toast({ text: "Thêm vào giỏ hàng thành công", icon: "success" }),
        //        $("#itemshop").text(n.count))


        //    : $.toast({
        //        text: "Quá trình thực hiện không thành công",
        //        icon: "error",
        //    });
    });
}


function ProductDetailJs() {

    $('.product-detail-content .see-open').click(function () {
        $(this).removeClass("active");
        $(".product-detail-content .see-close").addClass("active");
        $(".product-detail-content .content").addClass("active");
    })

    $('.product-detail-content .see-close').click(function () {
        $(this).removeClass("active");
        $(".product-detail-content .see-open").addClass("active");
        $(".product-detail-content .content").removeClass("active");

        let t = $('#thong-tin-san-pham').position().top; //lấy vị trí của phần trang web gán cho biến t
        $('html,body').stop().animate({ scrollTop: t }); //cuộn trang đến phần với biến t
    })


    $(".nice-number").niceNumber();

    $("#orderQuantity").on("change", function (e) {
            const quantity = $(this).val();
            const price = $("[name=productPrice]").val();
            const total = quantity * price;
            $("#total").text(total);
        });

    $("#orderForm").on("submit", function (e) {
        e.preventDefault();
        $.post("/dat-hang-nhanh", $(this).serialize(), function (data) {
            if (data === "True") {
                alert("Cảm ơn bạn đã đặt hàng. Chúng tôi sẽ liên hệ trong thời gian sớm nhất.");
            } else {
                alert("Quá trình thực hiện không thành công. Vui lòng thử lại sau ít phút.");
            }
        });
    });

    $('.product-img-list').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        infinite: true,
        arrows: true,
        autoplay: true,
        speed: 1500,
        autoplaySpeed: 5000,
        asNavFor: '.nav-img',
        prevArrow: ('<div class="slick-prev"><i class="fas fa-chevron-left"></i></div>'),
        nextArrow: ('<div class="slick-next"><i class="fas fa-chevron-right"></i></div>'),
    });

    $('.nav-img').slick({
        slidesToShow: 5,
        slidesToScroll: 1,
        asNavFor: '.product-img-list',
        infinite: true,
        //autoplay: true,
        arrows: false,
        speed: 1000,
        focusOnSelect: true,
        pauseOnHover: true,
        vertical: true,
        responsive: [
            {
                breakpoint: 767,
                settings: {
                    vertical: false,
                    slidesToShow: 5,
                    slidesToScroll: 1,
                }
            },
        ]
    });

    $("#formBookProduct").on("submit", function (e) {
        e.preventDefault();
        $.post("/gio-hang/them-vao-gio-hang", $(this).serialize(), function (data) {
            if (data.result === 1) {
                $.toast({
                    text: "Thêm vào giỏ hàng thành công",
                    icon: "success",
                    position: "top-center"
                });
                $("#itemshop").text(data.count);
            } else {
                $.toast("Quá trình thực hiện không thành công");
            }
        });
    });

   

    //$('.product-review-list').slick({
    //    slidesToShow: 4,
    //    slidesToScroll: 1,
    //    infinite: true,
    //    arrows: true,
    //    autoplay: true,
    //    speed: 1500,
    //    autoplaySpeed: 5000,
    //    prevArrow: ('<div class="slick-prev"><i class="fas fa-chevron-left"></i></div>'),
    //    nextArrow: ('<div class="slick-next"><i class="fas fa-chevron-right"></i></div>'),
    //});


    $(".product-review-list").owlCarousel({
        margin: 30,
        nav: true,
        autoplay: true,
        lazyLoad: true,
        navText: ["<i class='fal fa-angle-left'></i>", "<i class='fal fa-angle-right'></i>"],
        responsive: {
            0: {
                margin: 10,
                items: 1
            },
            600: {
                margin: 20,
                items: 3
            },
            1000: {
                items: 3
            }
        }
    });

    
    
}

function ShowMenu() {
    const x = document.getElementById("menu");
    if (x.className === "header-bottom py-lg-3") {
        x.className += " show-menu";
    } else {
        x.className = "header-bottom py-lg-3";
    }
}


function showSearch() {
    $(".header-search").toggleClass("active");
}

function CartJs() {


    $("#discount-from").on("submit", function (e) {
        e.preventDefault();

        var code = $('input[name="code-discount"]').val();

        

        UpdateCode(code)
    });

  /*  $(".nice-number").niceNumber();*/

    $('input[type="number"]').niceNumber({
        onDecrement: function (input, number, object) {
            //UpdateToCard()
            UpdateToCard($(input).attr('id-value'), -1);
            return false;
        },
        onIncrement: function (input, number, object) {
            UpdateToCard($(input).attr('id-value'), 1);
            return false;
        }
    });
}

function UpdateCode(code) {


/*    console.log(code)*/

    $.ajax({
        type: "Post",
        url: "/ShoppingCart/GetCode",
        data: { code: code},
        success: function (res) {
            if (res.Status == 1) {

                $('#tag-code').val(code);

                sessionStorage.setItem("code", code);

                var cartTotal = parseFloat($("[data-item='cart-total']").data("amount"));

                var FinalTotal = Number($("#finalPrices").text().replace(/,/g, ''));

                var shipFee = Number($("[data-item='ship-fee']").text().replace(/,/g, ''))

                if (res.PercentDiscount > 0) {
                    var newPrices = cartTotal - ((res.PercentDiscount * cartTotal) / 100) + shipFee;
                    var PricesDiscount = (res.PercentDiscount * cartTotal) / 100;
                    $("#prices-discount").text(PricesDiscount.toLocaleString());

                    $("#finalPrices").text(newPrices.toLocaleString());

                    $("#PricesDiscount").val(PricesDiscount);


                }
                else {
                    var newPrices = cartTotal - res.totalMoneyItem + shipFee;

                    var PricesDiscount = res.totalMoneyItem;

                    $("#prices-discount").text(PricesDiscount.toLocaleString());


                    $("#finalPrices").text(newPrices.toLocaleString());
                }

                /*  console.log($("#finalPrices").text())*/
                console.log(finalPrices)

                $.toast({
                    heading: res.Msg,
                    position: "bottom-right",
                    icon: "success"
                });
              /*  location.reload();*/
            }
            else {
                $.toast({
                    heading: res.Msg,
                    position: "bottom-right",
                    icon: "error"
                });

            }
        }
    });
}

function UpdateToCard(id, changeValue) {
    $.ajax({
        type: "Post",
        url: "/ShoppingCart/UpdateCartV2",
        data: { productId: id, changeValue },
        success: function (res) {
            if (res) {
                let shipFee = Number($("[data-item='ship-fee']").text().replace(/,/g, ''));
                $("#count-cart").html(res.totalItem);
                let old = $('input[name="priceOld"]').val();

                console.log(old * res.itemCount);
                $("[data-price-old=" + id + "]").text((old * res.itemCount).toLocaleString());
                $("[data-cart-item=" + id + "]").text(res.totalMoneyItem.toLocaleString());
                /*$("#finalPrices").html(res.totalMoneyString);*/
                $("#finalPrices").text((res.totalMoney + shipFee).toLocaleString());
              /*  $("#finalPrices").html(res.totalMoneyString);*/
                $.toast({
                    heading: res.Msg,
                    position: "bottom-right",
                    icon: "success"
                });
                location.reload();
            }
            else {
                $.toast({
                    heading: res.Msg,
                    position: "bottom-right",
                    icon: "error"
                });
            }
        }
    });
}