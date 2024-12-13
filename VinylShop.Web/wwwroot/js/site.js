function AddToCart(productId) {

    $.ajax({
        type: "GET",
        url: "/Cart/AddToCart",
        data: { productId: productId },
        error: function () {
            alert("An js error occurred!");
        },
        success: function () {
            var text = document.getElementById("product-cart-button-text-" + productId);
            if (text) {
                text.innerHTML = "In cart";
            }

            var img = document.getElementById("product-cart-button-img-" + productId);
            if (img) {
                img.src = "/images/check.png";
            }           
        }
    });
}