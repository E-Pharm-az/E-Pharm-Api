# UserController

## Base URL
`https://api.e-pharm.co/api/user`

## Endpoints

### 1. Get All Users

- **URL:** `/`
- **Method:** `GET`
- **Headers:**
    - No explicit token required in headers (Token is automatically passed via HTTP-only cookie)
    - `Authorization: Bearer {token}` (Admin role required)
- **Response:**

    - **Status Code:** `200 OK`
    - **Body:**
      ```json
      [
        {
          "id": "string",
          "email": "string",
          "firstName": "string",
          "lastName": "string",
          "address": "string",
          "district": "string",
          "city": "string",
          "zip": 12345
        }
      ]
      ```

    - **Description:** Retrieves a list of all users.

### 2. Get User By ID

- **URL:** `/{userId}`
- **Method:** `GET`
- - **Headers:**
  - No explicit token required in headers (Token is automatically passed via HTTP-only cookie)
  - `Authorization: Bearer {token}`
- **Response:**

    - **Status Code:** `200 OK`
    - **Body:**
      ```json
      {
        "id": "string",
        "email": "string",
        "firstName": "string",
        "lastName": "string",
        "address": "string",
        "district": "string",
        "city": "string",
        "zip": 12345
      }
      ```

    - **Description:** Retrieves a specific user by ID. Users can only access their own data unless they have an Admin role.

### 3. Register User

- **URL:** `/register`
- **Method:** `POST`
- **Request:**

    - **Content-Type:** `application/json`
    - **Body:**
      ```json
      {
        "email": "string"
      }
      ```

    - **Description:** Registers a new user. The email must be unique.

- **Response:**

    - **Status Code:** `200 OK`
    - **Description:** Successfully registered the user.

### 4. Initialize User

- **URL:** `/initialize`
- **Method:** `POST`
- **Request:**

    - **Content-Type:** `application/json`
    - **Body:**
      ```json
      {
        "email": "string",
        "code": 123456,
        "firstName": "string",
        "lastName": "string",
        "address": "string",
        "district": "string",
        "city": "string",
        "zip": 12345,
        "password": "string"
      }
      ```

    - **Description:** Initializes a user with additional details. Requires email confirmation.

- **Response:**

    - **Status Code:** `200 OK`
    - **Description:** Successfully initialized the user.

### 5. Confirm Email

- **URL:** `/confirm-email`
- **Method:** `POST`
- **Request:**

    - **Content-Type:** `application/json`
    - **Body:**
      ```json
      {
        "email": "string",
        "code": 123456
      }
      ```

    - **Description:** Confirms a user’s email address.

- **Response:**

    - **Status Code:** `200 OK`
    - **Body:**
      ```json
      "Email confirmed successfully"
      ```

    - **Description:** Email confirmation was successful.

### 6. Resend Confirmation Email

- **URL:** `/resend-confirmation-email`
- **Method:** `POST`
- **Request:**

    - **Content-Type:** `application/json`
    - **Body:**
      ```json
      {
        "email": "string"
      }
      ```

    - **Description:** Resends a confirmation email to the user.

- **Response:**

    - **Status Code:** `200 OK`
    - **Description:** Confirmation email resent successfully.

### 7. Register Admin

- **URL:** `/register/admin`
- **Method:** `POST`
- **Headers:**
    - No explicit token required in headers (Token is automatically passed via HTTP-only cookie)
    - `Authorization: Bearer {token}` (SuperAdmin role required)
- **Request:**

    - **Content-Type:** `application/json`
    - **Body:**
      ```json
      {
        "email": "string"
      }
      ```

    - **Description:** Registers a new admin. The email must be unique.

- **Response:**

    - **Status Code:** `200 OK`
    - **Description:** Successfully registered the admin.

### 8. Update User

- **URL:** `/{id}`
- **Method:** `PUT`
- **Headers:**
    - No explicit token required in headers (Token is automatically passed via HTTP-only cookie)
    - `Authorization: Bearer {token}`
- **Request:**

    - **Content-Type:** `application/json`
    - **Body:**
      ```json
      {
        "email": "string"
      }
      ```

    - **Description:** Updates user details. Only the user themselves or an Admin can update the details.

- **Response:**

    - **Status Code:** `200 OK`
    - **Body:**
      ```json
      "User updated successfully."
      ```

    - **Description:** User details updated successfully.

### 9. Delete User

- **URL:** `/{userId}`
- **Method:** `DELETE`
- **Headers:**
    - No explicit token required in headers (Token is automatically passed via HTTP-only cookie)
    - `Authorization: Bearer {token}`

  - **Description:** Only the user themselves or an Admin can delete the user.

- **Response:**

    - **Status Code:** `204 No Content`
    - **Description:** User successfully deleted.


### 10. Initiate Password Change

- **URL:** `/initiate-password-change`
- **Method:** `POST`
- **Request:**

    - **Content-Type:** `application/json`
    - **Body:**
      ```json
      {
        "email": "string"
      }
      ```

    - **Description:** Initiates a password change process for the user.

- **Response:**

    - **Status Code:** `200 OK`
    - **Description:** Password change initiation successful.

### 11. Change Password

- **URL:** `/change-password`
- **Method:** `POST`
- **Request:**

    - **Content-Type:** `application/json`
    - **Body:**
      ```json
      {
        "email": "string",
        "code": 123456,
        "password": "string"
      }
      ```

    - **Description:** Changes the user’s password using the provided code.

- **Response:**

    - **Status Code:** `200 OK`
    - **Description:** Password changed successfully.
