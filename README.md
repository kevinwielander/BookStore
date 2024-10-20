## BookStore API


The BookStore API is a RESTful service designed to manage a collection of books and their audit logs. It provides endpoints for performing CRUD operations on books and retrieving detailed audit logs of all changes made to the book inventory and being able to filter, order and group them.

### How to Start

1. Ensure you have .NET 8.0 SDK installed on your system.
2. Clone the repository to your local machine.
3. Navigate to the project directory in your terminal.
4. Run the following command to start the API:
   ```
   dotnet run
   ```
5. To explore the API endpoints, navigate to `https://localhost:5075/swagger` in your web browser to access the Swagger UI.

### How to Use

#### Books Endpoints

- **Get All Books**
    - **Endpoint:** `GET /api/v1/books`
    - **Usage:** Retrieve a list of all books in the store.

- **Get Book by ISBN**
    - **Endpoint:** `GET /api/v1/books/{isbn}`
    - **Usage:** Retrieve details of a specific book by its ISBN.

- **Add Book**
    - **Endpoint:** `POST /api/v1/books`
    - **Usage:** Add a new book to the store.
    - **Body:** Send a JSON object with book details (ISBN, title, description, publish date, authors).

- **Update Book**
    - **Endpoint:** `PUT /api/v1/books/{isbn}`
    - **Usage:** Update an existing book's information.
    - **Body:** Send a JSON object with updated book details.

- **Delete Book**
    - **Endpoint:** `DELETE /api/v1/books/{isbn}`
    - **Usage:** Remove a book from the store.

#### Audit Logs Endpoint

- **Get Audit Logs**
    - **Endpoint:** `GET /api/v1/book-audits`
    - **Usage:** Retrieve audit logs with various filtering and pagination options.
    - **Query Parameters:**
        - `filterKey`: Key to filter on (e.g., "isbn", "action")
        - `filterValue`: Value to filter by
        - `orderKey`: Key to order by (e.g., "changeTime", "isbn", "action")
        - `isDescending`: Boolean to determine sort order
        - `groupByKey`: Key to group results by
        - `pageNumber`: Page number for pagination (default: 1)
        - `pageSize`: Number of items per page (default: 10)