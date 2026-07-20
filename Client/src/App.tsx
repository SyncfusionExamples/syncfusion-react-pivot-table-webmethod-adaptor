import * as React from 'react';
import { PivotViewComponent, CellEditSettings } from '@syncfusion/ej2-react-pivotview';
import { DataManager, WebMethodAdaptor } from '@syncfusion/ej2-data';
import type { DataSourceSettingsModel } from '@syncfusion/ej2-pivotview/src/model/datasourcesettings-model';
import type { BeginDrillThroughEventArgs } from '@syncfusion/ej2-pivotview';
import './App.css';

function App(): React.ReactElement {
    // Configure DataManager with WebMethodAdaptor.
    const data: DataManager = new DataManager({
        url: 'http://localhost:5006/api/Orders',
        insertUrl: 'http://localhost:5006/api/Orders/Insert',
        updateUrl: 'http://localhost:5006/api/Orders/Update',
        removeUrl: 'http://localhost:5006/api/Orders/Remove',
        // crudUrl:'http://localhost:5006/api/Orders/CrudUpdate',
        adaptor: new WebMethodAdaptor()                // Specify UrlAdaptor for custom REST API.
    });

    const dataSourceSettings: DataSourceSettingsModel = {
        dataSource: data,
        expandAll: false,
        rows: [{ name: 'CustomerID' }],
        columns: [{ name: 'OrderID' }],
        values: [{ name: 'Freight' }],
        formatSettings: [{ name: 'Freight', format: 'N0' }],
    };

    // Enable editing functionality
    const editSettings: CellEditSettings = {
        allowEditing: true,    // Enables the Edit button and allows users to modify existing records.
        allowAdding: true,     // Enables the Add button and allows users to create new records.
        allowDeleting: true,   // Enables the Delete button and allows users to remove records.
        mode: 'Normal'         // Uses Normal mode (popup dialog) for editing; other options: 'Dialog', 'Batch', 'CommandColumn'.
    };


    const pivotObj = React.useRef<PivotViewComponent>(null);

    // Configure beginDrillThrough event to set the primary key for CRUD operations
    function beginDrillThrough(args: BeginDrillThroughEventArgs) {
        // Iterate through all columns in the drill-through grid
        for (var i = 0; i < args.gridObj.columns.length; i++) {
            // Check if the current column is the primary key column
            if (args.gridObj.columns[i].field === "OrderID") {
                // Mark this column as the primary key
                // This tells DataManager to use this column's value to uniquely identify records
                args.gridObj.columns[i].isPrimaryKey = true;
            } else {
                // Make all other columns visible so users can view and edit them
                args.gridObj.columns[i].visible = true;
                // Configure the edit type for date field to use a date picker for editing
                if (args.gridObj.columns[i].field === 'OrderDate' || args.gridObj.columns[i].field === 'ShippedDate') {
                    args.gridObj.columns[i].editType = 'datetimepickeredit';
                }
            }
        }
    }

    return (
        <div className='control-section' style={{ margin: 100 }}>
            <PivotViewComponent ref={pivotObj} id='PivotView' height={350} width={700} dataSourceSettings={dataSourceSettings} editSettings={editSettings} beginDrillThrough={beginDrillThrough}>
            </PivotViewComponent>
        </div>
    );
}

export default App;