<%@ page contentType="text/html;charset=UTF-8" language="java" %>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<html>
<head>
    <title>Product List</title>
    <link rel="stylesheet" href="assets/style.css">
</head>
<body>
<h2>Product List</h2>
<table>
    <thead>
        <th>Name</th>
        <th>Price</th>
        <th>Quantity</th>
        <th>Color</th>
        <th>Description</th>
        <th>Category</th>
    <th>Action</th>
    </thead>
    <tbody>
    <c:forEach var="e" items="${result}">
        <tr>
            <td>${e.name}</td>
            <td>${e.price}</td>
            <td>${e.quantity}</td>
            <td>${e.color}</td>
            <td>${e.description}</td>
            <td>${e.categoryName}</td>
            <th>
                <a href="/form.jsp">Edit</a>
                <a href="#">Delete</a>
            </th>
        </tr>
    </c:forEach>
    </tbody>
</table>
</body>
</html>
