import { Component, Inject, EventEmitter, OnInit, Output, Input } from '@angular/core';
import { DataService } from '../../../services/data.service'
import { Good } from '../../../model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { GoodEditResetModalContent } from './good-edit-resetmodal.component'
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
    selector: 'good-edit-header',
    templateUrl: './good-edit-header.component.html',
    styleUrls: ['./../../eshop.component.css']
})

export class EditGoodHeaderComponent {

    @Output() goodWasUpdated: EventEmitter<Good>;
    @Output() goodWasCreated: EventEmitter<Good>;

    private _isValidForm: boolean;
    private _editableGood: Good;
    private _primaryGood; Good;

    private _isEnabledCancelButton: boolean = false;
    private _isEnabledSaveButton: boolean = false;

    constructor(@Inject(DataService) private dataService: DataService,
        private modalService: NgbModal) {
        this.goodWasUpdated = new EventEmitter<Good>();
        this.goodWasCreated = new EventEmitter<Good>();
        this._editableGood = new Good(null, "", 0.0, 0, null, null);
        this._primaryGood = this._editableGood;
    }

    onChangeFormValidation(isValid: boolean) {
        console.log("form changed", isValid);
        this._isValidForm = isValid;
        let editableGood = JSON.stringify(this._editableGood);
        let primaryGood = JSON.stringify(this._primaryGood);
        this._isEnabledSaveButton = isValid && editableGood != primaryGood;
        this._isEnabledCancelButton = editableGood != primaryGood;
    }

    saveGood(): void {
        this.sendGoodToSave().subscribe();
    }

    sendGoodToSave(): Observable<any> {
        if (this._isValidForm) {
            if (this._editableGood.id != null) {
                // обновление
                return this.dataService.updateProduct(this._editableGood).pipe(
                    map((data: Good) => {
                        this._primaryGood = data;
                        this.goodWasUpdated.emit(this._primaryGood);
                        this._isEnabledSaveButton = false;
                        this._isEnabledCancelButton = false;
                    })
                );
            }
            else {
                // создание
                return this.dataService.createProduct(this._editableGood).pipe(
                    map((data: Good) => {
                        this._primaryGood = data;
                        this._isEnabledSaveButton = false;
                        this._isEnabledCancelButton = false;
                        this.goodWasCreated.emit(this._primaryGood);
                    })
                );
            }
        }
        return Observable.create();
    }

    newGoodWasSelected(good: Good): void {
        let editableGood = JSON.stringify(this._editableGood);
        let primaryGood = JSON.stringify(this._primaryGood);
        if (editableGood != primaryGood) {
            // спрашиваем, нужно сохранить товар, или можно затирать изменения
            this.openConfirmResetModal(good);
        }
        else {
            this._primaryGood = good;
            this._editableGood = (JSON.parse(JSON.stringify(good)));
        }
    }

    resetChanges() {
        this._editableGood = (JSON.parse(JSON.stringify(this._primaryGood)));
    }

    openConfirmResetModal(newGood: Good) {
        const modalRef = this.modalService.open(GoodEditResetModalContent, { backdrop: "static" });
        modalRef.result.then((userResponse) => {
            if (userResponse == 'save') {
                this.sendGoodToSave().subscribe(g => {
                    this._primaryGood = newGood;
                    this._editableGood = (JSON.parse(JSON.stringify(newGood)))
                });
            }
            else {
                this._primaryGood = newGood;
                this._editableGood = (JSON.parse(JSON.stringify(newGood)));
            }
        });
    }
}

