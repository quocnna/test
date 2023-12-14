<%@ page contentType="text/html;charset=UTF-8" language="java" %>
<html>
<head>
    <title>Customer Form</title>
    <link rel="stylesheet" href="${pageContext.request.contextPath}/webjars/bootstrap/5.3.2/css/bootstrap.min.css">
</head>
<body class="container">
<h1>Customer Form</h1>
<form method="post" action="${pageContext.request.contextPath}/customers">
    <div class="mb-3">
        <label class="form-label">Name</label>
        <input class="form-control" name="name" value="${customer.name()}">
    </div>
    <div class="mb-3">
        <label class="form-label">Birthday</label>
        <input type="date" name="birthday" class="form-control" value="${customer.birthday()}">
    </div>
    <div class="mb-3">
        <label class="form-label">Gender</label>
        <div class="form-check">
            <input class="form-check-input" type="radio" name="gender"
                   value="true" ${customer.gender() ? 'checked' : ''}>
            <label class="form-check-label">
                Male
            </label>
        </div>
        <div class="form-check">
            <input class="form-check-input" type="radio" name="gender" ${customer.gender() ? 'checked' : ''}>
            <label class="form-check-label">
                Female
            </label>
        </div>
    </div>
    <div class="mb-3">
        <label class="form-label">Card</label>
        <input class="form-control" name="card" value="${customer.card()}">
    </div>
    <div class="mb-3">
        <label class="form-label">Phone</label>
        <input class="form-control" name="phone" value="${customer.phone()}">
    </div>
    <div class="mb-3">
        <label class="form-label">Email</label>
        <input class="form-control" name="email" value="${customer.email()}">
    </div>
    <div class="mb-3">
        <label class="form-label">Customer Type</label>
        <select name="customerTypeId" class="form-select">
            <option value="0">Open this select menu</option>
            <option value="1" ${customer.customerTypeId()==1 ? 'selected' : ''}>Gold</option>
            <option value="2" ${customer.customerTypeId()==2 ? 'selected' : ''}>Diamond</option>
            <option value="3" ${customer.customerTypeId()==3 ? 'selected' : ''}>Planium</option>
        </select>
    </div>
    <a href="${pageContext.request.contextPath}/customers" class="btn btn-danger">Cancel</a>
    <button name="id" value="${customer.id()}" class="btn btn-primary">Save</button>
</form>
</body>
</html>
