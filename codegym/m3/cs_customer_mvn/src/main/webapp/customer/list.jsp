<%@ taglib prefix="c" uri="jakarta.tags.core" %>
<%@ page contentType="text/html;charset=UTF-8" language="java" %>
<html>
<head>
    <title>Title</title>
    <link rel="stylesheet" href="${pageContext.request.contextPath}/webjars/bootstrap/5.3.2/css/bootstrap.min.css">
    <link rel="stylesheet" href="${pageContext.request.contextPath}/asset/css/main.css">
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.7/css/jquery.dataTables.min.css">
</head>
<body class="container">
<h1>Customer List</h1>
<div class="row">
    <div class="col">
        <a href="${pageContext.request.contextPath}/customer/form.jsp" class="btn btn-success">Add New</a>
    </div>
    <div class="col-4">
        <a onclick="location.href='/customers?q='+ searchName.value" class="btn btn-success float-end ms-2 me-2">Search</a>
        <input id="searchName" class="form-control-sm float-end" placeholder="Search Name">
    </div>
</div>
<table id="tblCustomer" class="table table-hover">
    <thead>
    <tr>
        <th>#</th>
        <th>Name</th>
        <th>Birthday</th>
        <th>Gender</th>
        <th>Phone</th>
        <th>Customer Type</th>
        <th>Action</th>
    </tr>
    </thead>
    <tbody>
    <c:forEach items="${customers}" var="c" varStatus="v">
        <tr>
            <td>${v.count}</td>
            <td>${c.name()}</td>
            <td>${c.birthday()}</td>
            <td>${c.gender() ? 'Male' : 'Female'}</td>
            <td>${c.phone()}</td>
            <td>${c.customerTypeName()}</td>
            <td>
                <button onclick="titleCustomer.textContent=${c.name()}; idDelete.value=${c.id()}" class="btn btn-danger"
                        data-bs-toggle="modal" data-bs-target="#deleteModal">Delete
                </button>
                <a href="${pageContext.request.contextPath}/customers?id=${c.id()}" class="btn btn-primary">Edit</a>
            </td>
        </tr>
    </c:forEach>
    </tbody>
</table>

<div class="modal fade" id="deleteModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h1 class="modal-title fs-5" id="titleCustomer">Modal title</h1>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                Are you sure to delete this record?
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                <button id="idDelete" onclick="deleteCustomer(this.value)" type="button" class="btn btn-danger">Delete
                </button>
            </div>
        </div>
    </div>
</div>

<script src="webjars/bootstrap/5.3.2/js/bootstrap.bundle.js"></script>
<script src="https://code.jquery.com/jquery-3.7.0.js"></script>
<script src="https://cdn.datatables.net/1.13.7/js/jquery.dataTables.min.js"></script>
<script>
    new DataTable('#tblCustomer', {
        pagingType: 'full_numbers',
        lengthMenu: [
            [2, 5, 10, -1],
            [2, 5, 10, 'All']
        ],
        searching: false,
        lengthChange: false
    });

    function deleteCustomer(id) {
        fetch('/customers?id=' + id, {
            method: 'DELETE'
        })
            .then(response => {
                if (response.ok) {
                    location.href = '/customers';
                } else {
                    throw new Error('Error deleting user');
                }
            })
    }
</script>
</body>
</html>
