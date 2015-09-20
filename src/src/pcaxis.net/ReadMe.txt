This is a .net Provider for PcAxis files that uses an SQLite database to perform the queries on.

In order to use it, the App.Config or the Web.Config files of the applications using it, must have the following lines registered:

	<system.data>
		<DbProviderFactories>
			<add name="Pc Axis Provider"
				 invariant="org.estat.PcAxis.PcAxisProvider"
				 description=".Net Framework Data Provider for Pc-Axis"
				 type="org.estat.PcAxis.PcAxisProvider.PcAxisFactory, PcAxis, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
		</DbProviderFactories>
	</system.data>

 Be aware that this is not a fully implemented provider, so there might be cases that it won't work because there are some methods that are not supported.
 
 In particular, DbProviderFactory.CreateCommand() method is not supported. You can use DbConnection.CreateCommand() instead.
 
 GetSchema() methods are not fully supported neither.
 The following methods work for it:
 
 1)GetSchema("Tables") returns this DataTable:
 
 TABLE_NAME			FILEPATH
 -----------------------------------------------------------------
 The table name		The exact file path of the local Pc-Axis file
 
 
 2)GetSchema("Columns") returns this DataTable:
 
 TABLE_NAME			COLUMN_NAME				DATA_TYPE
 -------------------------------------------------------------------------
 The table name		The column name			The data type (always VARCHAR)

 3)GetSchema("Columns", new string[]{null,null, "TableName"}) will also work and returns the columns of the specified table like 2).
 