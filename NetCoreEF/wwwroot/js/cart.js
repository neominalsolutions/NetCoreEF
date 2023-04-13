
function addToCart(productId, productName) {

  const cartObject = {
    Name: productName,
    Id: productId
  }

  console.log('cartObject', cartObject);

  $.ajax({
    url: '/Product/AddToCart',
    method: 'POST',
    contentType: 'application/json',
    dataType: 'text',
    data: JSON.stringify(cartObject),
    success: (htmlContent) => {
      console.log('htmlContent', htmlContent);
      $("#cart").html(htmlContent);
    },
    error: (err) => {
      console.log('err', err);
    }
  })

}
