import { Component, Inject, OnInit } from '@angular/core';
import { Subject } from 'rxjs'
import { LoaderService } from '../services/loader.service'

@Component({
    selector: 'loader',
    templateUrl: './loader.component.html',
    styleUrls: ['./loader.component.css']
})

export class LoaderComponent implements OnInit {

    loading: boolean;
    constructor(private loaderService: LoaderService) {
        this.loaderService.isLoading.subscribe((v) => {
            this.loading = v;
        });
    }

    ngOnInit() {
    }
}

