import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'good-list-reset-modal',
    templateUrl: './good-list-confirmdeletemodal.component.html',
    styleUrls: ['./../../eshop.component.css']
})

export class GoodListConfirmDeleteModalContent {

    constructor(public modal: NgbActiveModal) { }
}

