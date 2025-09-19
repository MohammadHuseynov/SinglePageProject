/**
 * The DOMContentLoaded event fires when the initial HTML document has been completely loaded.
 * This is the standard entry point for running the script's setup functions.
 */
document.addEventListener("DOMContentLoaded", function () {
    initializeEventListeners();
    LoadProducts();
});

// --- Constants and Global Element References ---
const productForm = document.getElementById('productForm');
const titleInput = document.getElementById('title');
const unitPriceInput = document.getElementById('unitPrice');
const tableBody = document.getElementById('productTableBody');
const userFeedback = document.getElementById('userFeedback');
const btnRefresh = document.getElementById('btnRefresh');
const btnAdd = document.getElementById('btnAdd');
const btnEdit = document.getElementById('btnEdit');
const btnDelete = document.getElementById('btnDelete');


let selectedProductIds = []; //stores the id of the selected products


const API_URL = "/Product";// The base URL for the controller actions

/**
 * Attaches event listeners to the interactive elements on the page.
 */
function initializeEventListeners() {

    btnAdd.addEventListener('click', Add);

    btnEdit.addEventListener('click', Edit);

    btnDelete.addEventListener('click', Delete);

    btnRefresh.addEventListener('click', LoadProducts);

}

// --- Data Loading Function ---

/**
 * Fetches all products from the API and renders them in the HTML table.
 */
function LoadProducts() {

    setFeedback("Loading products...", "info");
    tableBody.innerHTML = ""; // Clear existing table rows.

    fetch(`${API_URL}/GetAll`)
        .then(response => {
            if (!response.ok) {
                throw new Error(`Network error: ${response.status} - ${response.statusText}`);
            }
            return response.json();
        })
        .then(products => {
            clearFeedback();
            if (products.length === 0) {
                renderEmptyRow();
                return;
            }
            products.forEach(product => renderProductRow(product));

        })
        .catch(error => {
            console.error("Failed to load products:", error);
            setFeedback(`Error loading products: ${error.message}`, "danger");
        });
}

// --- Form and Data Submission Functions ---

/**
 * Handles the form submission event.
 * It validates the form and calls the function to add a new product.
 * @param {Event} event - The form submission event object.
 */
function AddProduct(event) {
    event.preventDefault(); // Prevent the default full-page reload.

    if (!validateForm()) {
        return;
    }

    const productDto = {
        title: titleInput.value,
        unitPrice: unitPriceInput.value,
    };

    // Send the new product data to the server.
    fetch(`/Product/Post`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        body: JSON.stringify(productDto)
    })
        .then(response => {
            if (!response.ok) {
                return response.json().then(errorData => {
                    throw new Error(errorData.message || "The server returned an error.");
                });
            }
            return response.json();
        })
        .then(data => {
            setFeedback("Product saved successfully!", "success");
            resetForm();
            LoadProducts();
        })
        .catch(error => {
            console.error("Failed to add product:", error);
            setFeedback(`Error saving product: ${error.message}`, "danger");
        });
}


/**
 * Handles the click of a delete button, asks for confirmation, and sends the delete request.
 * @param {string} id - The ID of the product to delete.
 */
function DeleteProduct(id) {
    if (!confirm("Are you sure you want to delete this product?")) {
        return;
    }

    setFeedback("Deleting product...", "info");

    //The product ID is now part of the URL itself.
    fetch(`/Product/Delete/${id}`, {
        method: "DELETE" // The method is DELETE. No headers or body are needed.
    })
        .then(response => {
            if (response.ok) {
                return;
            } else {

                return response.json().then(errorData => {
                    throw new Error(errorData.message || "Failed to delete the product.");
                });
            }
        })
        .then(() => {
            setFeedback("Product deleted successfully!", "success");

            LoadProducts();
            resetForm();
        })
        .catch(error => {
            console.error("Deletion failed:", error);
            setFeedback(`Error deleting product: ${error.message}`, "danger");
        });

}


/**
 * @param {string} id - The ID of the product to Edit.
 */
function EditProduct(id) {



    const productDto = {
        Id: id,
        Title: titleInput.value,
        UnitPrice: unitPriceInput.value,
    };

    fetch(`/Product/Put/${id}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        body: JSON.stringify(productDto)
    })
        .then(response => {
            if (response.ok) {
                alert("Product updated successfully");
                LoadProducts();
            }
            else {
                return response.json().then(err => {
                    throw new Error(err.message || "Error updating product");
                });
            }
        })
        .catch(error => {
            console.error("Update failed:", error);
            alert("Failed to update product: " + error.message);
        });

    resetForm();
    LoadProducts();



}



/**
 * This function is called whenever a user clicks a product row checkbox.
 * It manages the selectedProductIds array and updates the input form based on the selection.
 * @param {HTMLInputElement} checkbox - The checkbox element that was clicked.
 * @param {string} id - The ID of the product in the row.
 */
function SelectedProduct(checkbox, id) {

    if (checkbox.checked) {
        if (!selectedProductIds.includes(id)) {
            selectedProductIds.push(id);
        }
    } else {
        selectedProductIds = selectedProductIds.filter(productId => productId !== id);
    }


    if (selectedProductIds.length === 1) {

        const singleSelectedId = selectedProductIds[0];

        // Find the table row (<tr>) for this product using its unique ID.
        const productRow = document.getElementById(`product-row-${singleSelectedId}`);


        // From within that row, find the cells for title and unit price.
        // We use querySelector and nth-child to be precise.
        // ':nth-child(2)' gets the second <td>, which is the Title.
        // ':nth-child(3)' gets the third <td>, which is the Unit Price.
        const titleCell = productRow.querySelector('td:nth-child(2)');
        const priceCell = productRow.querySelector('td:nth-child(3)');

        // Extract the text from the cells and populate the form inputs.
        titleInput.value = titleCell.innerText;
        unitPriceInput.value = priceCell.innerText;

    } else {
        resetForm();
    }

    console.log("Selected IDs:", selectedProductIds);
    UpdateButtonStates();
}

/**
 * Updates the Save, Edit, and Delete buttons
 */
function UpdateButtonStates() {
    const selectedCount = selectedProductIds.length;

    btnAdd.disabled = selectedCount > 0;

    btnEdit.disabled = selectedCount !== 1;

    btnDelete.disabled = selectedCount === 0;
}


// --- Event Handler Functions ---

/**
 * Handles the click event for the "Save" (Add) button.
 */
function Add(event) {
    // This handler's job is to call the main AddProduct function,
    // passing along the event object needed for preventDefault().
    AddProduct(event);
}

/**
 * Handles the click event for the "Edit" button.
 */
function Edit() {
    // This handler's job is to check the state and then call the main EditProduct function.
    if (selectedProductIds.length === 1) {
        const idToEdit = selectedProductIds[0];
        EditProduct(idToEdit);
    }
}

/**
 * Handles the click event for the "Delete" button.
 */
function Delete() {
    // This handler's job is to check the state and then call the main DeleteProduct function.
    if (selectedProductIds.length > 0) {
        // This example deletes the first selected item for simplicity.
        const idToDelete = selectedProductIds[0];
        DeleteProduct(idToDelete);
    }
}


// --- UI Helper Functions ---

/**
 * Renders a single product row in the HTML table.
 */
function renderProductRow(product) {
    const rowHtml = `
        <tr id="product-row-${product.id}">

            <td style="width: 10px;">
            <input type="checkbox" class=form-check-input" onclick="SelectedProduct(this, '${product.id}')" />
            </td>

            <td>${escapeHTML(product.title)}</td>
            <td>${product.unitPrice.toFixed(2)}</td>
            
        </tr>
    `;
    tableBody.insertAdjacentHTML("beforeend", rowHtml);
}

/**
 * Displays a message in the table when no data is available.
 */
function renderEmptyRow() {
    tableBody.innerHTML = '<tr><td colspan="3" class="text-center">No products found.</td></tr>';
}

/**
 * Displays feedback messages to the user.
 */
function setFeedback(message, type) {
    userFeedback.innerText = message;
    userFeedback.className = `alert alert-${type}`;
}

/**
 * Clears the feedback message area.
 */
function clearFeedback() {
    userFeedback.innerText = "";
    userFeedback.className = "";
}

/**
 * Resets the form inputs after a successful submission.
 */
function resetForm() {

    UpdateButtonStates();
    productForm.reset();
    titleInput.classList.remove('is-invalid');
    unitPriceInput.classList.remove('is-invalid');

}

/**
 * Basic client-side validation for the form.
 */
function validateForm() {
    let isValid = true;
    titleInput.classList.remove('is-invalid');
    unitPriceInput.classList.remove('is-invalid');

    if (titleInput.value.trim() === "") {
        titleInput.classList.add('is-invalid');
        isValid = false;
    }

    if (unitPriceInput.value === "" || unitPriceInput.value <= 0) {
        unitPriceInput.classList.add('is-invalid');
        isValid = false;
    }
    return isValid;
}

/**
 * Escapes HTML special characters to prevent Cross-Site Scripting (XSS).
 */
function escapeHTML(str) {
    return str.replace(/[&<>"']/g, function (match) {
        return {
            '&': '&amp;',
            '<': '&lt;',
            '>': '&gt;',
            '"': '&quot;',
            "'": '&#39;'
        }[match];
    });
}

// --- Placeholder functions for future implementation ---
function handleDetails(id) {
    console.log(`Details for product ID: ${id}`);
    // Future logic: Fetch details for this product and display in a modal.
}

