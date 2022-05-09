<%@ page contentType="text/html;charset=UTF-8" language="java" %>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<html>
<head>
    <title>Product List</title>
    <link rel="stylesheet" href="assets/style.css">
</head>
<body>
<h2>Product List</h2>
<a href="/form.jsp">Add new</a>
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
                <a href="?id=${e.id}">Edit</a>
                <a id="test" onclick="delete_confirm(${e.id})">Delete</a>
            </th>
        </tr>
    </c:forEach>
    </tbody>
</table>

<script>
    function delete_confirm(id){
        const choose = confirm("Are you sure you want to delete?")
        if(choose == true){
            alert(id);
            document.getElementById("test").href = "http://vnexpress.net";
        }
        else {
            alert("2");
            return;
        }
    }
</script>
</body>
</html>
