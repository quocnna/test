package cg.m3.cs_customer_mvn.model;

import java.util.Date;

public record Customer (int id, String name, Date birthday, Boolean gender, String card, String phone, String email
        , String address, int customerTypeId, String customerTypeName) {}
