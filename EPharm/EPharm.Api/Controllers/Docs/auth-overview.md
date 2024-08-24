# AuthController

## Base URL
`https://api.e-pharm.co/api/auth/{role}`

### Roles
- `customer`
- `pharmacy-admin`
- `pharmacy-staff`
- `admin`
- `super-admin`

## Endpoints

### 1. Login

- **URL:** `/login`
- **Method:** `POST`
- **Request:**

    - **Content-Type:** `application/json`
    - **Body:**
      ```json
      {
        "email": "string",
        "password": "string"
      }
      ```

- **Response:**

    - **Status Code:** `200 OK`
    - **Body:**
      ```json
      {
        "token": "string",
        "refreshToken": "string",
        "validTo": "string"
      }
      ```

    - **Cookies Set:**
      - `accessToken` with a short expiration time
      - `refreshToken` with a long expiration time

- **Description:** Logs in a user and sets authentication cookies (JWT inside HTTP-only cookies).

### 2. Refresh Token

- **URL:** `/refresh-token`
- **Method:** `GET`
- **Request:**

    - **Cookies:**
        - `accessToken` (string)
        - `refreshToken` (string)

- **Response:**

    - **Status Code:** `200 OK`
    - **Body:**
      ```json
      {
        "token": "string",
        "refreshToken": "string",
        "validTo": "string"
      }
      ```

    - **Cookies Updated:**
        - `accessToken` with a new expiration time
        - `refreshToken` with a new 7-day expiration

- **Description:** Refreshes the authentication tokens using the provided cookies.

### 3. Logout

- **URL:** `/logout`
- **Method:** `POST`
- **Response:**

    - **Status Code:** `200 OK`

- **Description:** Logs out a user by removing authentication cookies.
