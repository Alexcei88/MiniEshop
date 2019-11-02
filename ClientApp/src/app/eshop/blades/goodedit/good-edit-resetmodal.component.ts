import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'good-edit-reset-modal',
    templateUrl: './good-edit-resetmodal.component.html',
    styleUrls: ['./../../eshop.component.css']
})

export class GoodEditResetModalContent {

    constructor(public modal: NgbActiveModal) { }
}

