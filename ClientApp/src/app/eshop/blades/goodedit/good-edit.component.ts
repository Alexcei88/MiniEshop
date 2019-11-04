import { Component, Inject, EventEmitter, OnInit, Input, Output, ElementRef, ViewChild } from '@angular/core';
import { DataService } from '../../../services/data.service'
import { Good } from '../../../model';
import { EditGoodResetModalContent } from './good-edit-resetmodal.component'
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HeaderButton } from '../../blades/blade-header.component'
import { BladeConfig } from 'ngx-blade/esm5/ngx-blade';
import { NgxBladeComponent } from 'ngx-blade/esm5/src/app/modules/ngx-blade/ngx-blade.component';
import { EditGoodBodyComponent } from './good-edit-body.component'

@Component({
    selector: 'good-edit-blade',
    templateUrl: './good-edit.component.html',
    styleUrls: ['./../../eshop.component.css']
})

export class EditGoodComponent implements OnInit {

    public bladeConfig: BladeConfig = {
        closeButton: false,
        maximizeButton: false,
        isModal: false,
    };

    @Output() goodWasUpdated: EventEmitter<Good>;
    @Output() goodWasCreated: EventEmitter<Good>;

    private _isValidForm: boolean;
    private _editableGood: Good;
    public get editableGood() { return this._editableGood; }
    private _primaryGood: Good;

    private _isEnabledCancelButton: boolean = false;
    private _isEnabledSaveButton: boolean = false;

    private _buttons: HeaderButton[] = [];
    public get buttons() {return this._buttons; }


    private _blade: NgxBladeComponent;
    @ViewChild("editGoodBlade", { static: false }) set blade(blade: NgxBladeComponent) {
        if (blade !== undefined) {
            this._blade = blade;
            this._blade.onMaximize();
        }
    }

    private bodyComponent: EditGoodBodyComponent;
    @ViewChild('body', { static: false }) set _body(body: EditGoodBodyComponent) {
        if (body !== undefined) {
            this.bodyComponent = body;
        }
    }

    constructor(@Inject(DataService) private dataService: DataService,
        private modalService: NgbModal) {
        this.goodWasUpdated = new EventEmitter<Good>();
        this.goodWasCreated = new EventEmitter<Good>();
        this._editableGood = new Good(null, "", 0.0, 0, null, null);
        this._primaryGood = this._editableGood;
    }

    ngOnInit() {
        let saveGoodButton = new HeaderButton('fa fa-save fa-3x', "Сохранить", () => !this._isEnabledSaveButton);
        saveGoodButton.click.subscribe(g => this.saveGood());
        this._buttons.push(saveGoodButton);

        let removeButton = new HeaderButton('fa fa-times fa-3x', "Отменить", () => !this._isEnabledCancelButton);
        removeButton.click.subscribe(g => this.resetChanges());
        this._buttons.push(removeButton);
    }

    onFormValueChanges(evt: any) {
        this._isValidForm = evt.valid;
        this.updateButtonStates(evt.good);
    }

    updateButtonStates(newGood: Good) {
        let editableGood = JSON.stringify(newGood);
        let primaryGood = JSON.stringify(this._primaryGood);
        this._isEnabledSaveButton = this._isValidForm && editableGood != primaryGood;
        this._isEnabledCancelButton = editableGood != primaryGood;
    }

    saveGood(): void {
        this.sendGoodToSave().subscribe();
    }

    close() {
        if (this._blade != undefined) {
            this._blade.close();
        }
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
        if(this.bodyComponent != undefined) {
            this.bodyComponent.resetImage();
        }
    }

    openConfirmResetModal(newGood: Good) {
        const modalRef = this.modalService.open(EditGoodResetModalContent, { backdrop: "static" });
        modalRef.result.then((userResponse) => {
            if (userResponse == 'save') {
                this.sendGoodToSave().subscribe(g => {
                    this._primaryGood = newGood;
                    this._editableGood = (JSON.parse(JSON.stringify(newGood)))
                });
            }
            else {
                this.resetChanges();
                this._primaryGood = newGood;
                this._editableGood = (JSON.parse(JSON.stringify(newGood)));
            }
        });
    }

}

