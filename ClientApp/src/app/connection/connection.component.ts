import { Component, Inject, OnInit } from '@angular/core';
import { ConnectionService } from 'ng-connection-service';

@Component({
    selector: 'app-connection',
    templateUrl: './connection.component.html'
})

export class ConnectionComponent implements OnInit {

    isConnected: boolean = false;
    constructor(private connectionService: ConnectionService) {
        this.connectionService.monitor().subscribe(isConnected => {
            this.isConnected = isConnected;
        })
    }

    ngOnInit() {
    }
}

