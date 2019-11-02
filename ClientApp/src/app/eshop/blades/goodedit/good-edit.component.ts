import { Component, Inject, EventEmitter, OnInit, Input, Output, ElementRef, ViewChild } from '@angular/core';
import { DataService } from '../../../services/data.service'
import { Good } from '../../../model';
import { createImagePath } from '../../../common'
import { NgForm } from '@angular/forms';
import { BehaviorSubject } from 'rxjs'

@Component({
    selector: 'good-edit',
    templateUrl: './good-edit.component.html',
    styleUrls: ['./../../eshop.component.css']
})

export class EditGoodComponent implements OnInit {

    private _good: Good = new Good('', '', 0.0, 1, null, null);

    @Output() formChangeValid: EventEmitter<boolean>;

    ngOnInit() { }

    @Input('editableGood') set setGood(value: Good) {
        if (value != undefined) {
            this._good = value;
            if (this._editGoodForm != undefined) {
                // yourForm.reset(), yourForm.resetForm() don't work, but this does:
                this._editGoodForm.form.markAsPristine();
                this._editGoodForm.form.markAsUntouched();
                this._editGoodForm.form.updateValueAndValidity();
            }
        }
    }

    private _editGoodForm: NgForm;
    @ViewChild("editGoodForm", { static: false }) set content(content: NgForm) {
        if (content !== undefined) {
            this._editGoodForm = content;
            this._editGoodForm.statusChanges.subscribe(
                result => this.formChangeValid.emit(result === 'VALID' ? true : false)
            );
        }
    }

    @ViewChild('inputFile', { static: false }) imageInputVariable: ElementRef;
    reset() {
        this.imageInputVariable.nativeElement.value = '';
    }

    constructor(@Inject(DataService) private dataService: DataService) {
        this.formChangeValid = new EventEmitter<boolean>();
    }

    uploadFiles(event: any) {
        let files = event.target.files;
        this.dataService.uploadFile(files).subscribe((data: any) => {
            this._good.imageUrl = data.dbPath;
            this.formChangeValid.emit(true);
        });
    }

    createImgPath = (serverPath: string) => {
        return createImagePath(serverPath);
    }

}

