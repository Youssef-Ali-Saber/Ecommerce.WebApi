## Usage

Explore the various endpoints to manage products, users, shopping cart, wishlist, orders, payments, and shipments.

### Product Endpoints

1. **GET /api/products:** Retrieve a list of all products.
2. **GET /api/products/{productId}:** Retrieve details of a specific product.
3. **POST /api/products:** Create a new product (requires admin access).
4. **PUT /api/products/{productId}:** Update details of a specific product (requires admin access).
5. **DELETE /api/products/{productId}:** Delete a specific product (requires admin access).

### User Endpoints

1. **POST /api/users/register:** Register a new user.
2. **POST /api/users/login:** User login.
3. **GET /api/users/{userId}:** Get user details (requires authentication).
4. **PUT /api/users/{userId}:** Update user details (requires authentication).

### Cart Endpoints

1. **GET /api/cart:** Get the contents of the user's shopping cart (requires authentication).
2. **POST /api/cart/add/{productId}:** Add a product to the shopping cart (requires authentication).
3. **PUT /api/cart/update/{productId}:** Update the quantity of a product in the cart (requires authentication).
4. **DELETE /api/cart/remove/{productId}:** Remove a product from the cart (requires authentication).

### Wishlist Endpoints

1. **GET /api/wishlist:** Get the user's wishlist (requires authentication).
2. **POST /api/wishlist/add/{productId}:** Add a product to the wishlist (requires authentication).
3. **DELETE /api/wishlist/remove/{productId}:** Remove a product from the wishlist (requires authentication).

### Order Endpoints

1. **POST /api/orders:** Place a new order (requires authentication).
2. **GET /api/orders:** Get a list of the user's orders (requires authentication).
3. **GET /api/orders/{orderId}:** Get details of a specific order (requires authentication).
4. **PUT /api/orders/cancel/{orderId}:** Cancel a specific order (requires authentication).

### Payment Endpoints

1. **POST /api/payments:** Make a payment for an order (requires authentication).
2. **GET /api/payments/{paymentId}:** Get details of a specific payment (requires authentication).

### Shipment Endpoints

1. **POST /api/shipments:** Create a new shipment (requires admin access).
2. **GET /api/shipments/{shipmentId}:** Get details of a specific shipment (requires admin access).

## Authentication with JWT , Refresh Token and Identity

This project uses JWT (JSON Web Tokens) for authentication. When logging in, users receive an access token and a refresh token. Access tokens expire after a certain time, and the refresh token can be used to obtain a new access token without re-entering credentials.

1. **Obtaining JWT:**

   To obtain a JWT, users need to authenticate using their credentials.

   Example Request:

   ```bash
   curl -X POST -H "Content-Type: application/json" -d '{"username": "your-username", "password": "your-password"}' http://localhost:5000/api/users/login
   ```

   Example Response:

   ```json
   {
     "token": "your-access-token",
     "refreshToken": "your-refresh-token"
   }
   ```

2. **Refreshing JWT:**

   Users can refresh their JWT by sending a request with the refresh token.

   Example Request:

   ```bash
   curl -X POST -H "Content-Type: application/json" -d '{"refreshToken": "your-refresh-token"}' http://localhost:5000/api/token/refresh
   ```

   Example Response:

   ```json
   {
     "token": "new-access-token",
     "refreshToken": "new-refresh-token"
   }
   ```
