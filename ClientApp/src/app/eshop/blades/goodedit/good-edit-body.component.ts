import { Component, Inject, EventEmitter, OnInit, Input, Output, ElementRef, ViewChild } from '@angular/core';
import { DataService } from '../../../services/data.service'
import { Good } from '../../../model';
import { FileLink } from '../../../model';

import { createImagePath } from '../../../common'
import { NgForm } from '@angular/forms';

@Component({
    selector: 'good-edit-body',
    templateUrl: './good-edit-body.component.html',
    styleUrls: ['./../../eshop.component.css']
})

export class EditGoodBodyComponent implements OnInit {

    public good: Good = new Good('', '', 0.0, 1, new FileLink(null, null), null);

    @Output() formValueChanges: EventEmitter<any>;

    ngOnInit() { }

    @Input('editableGood') set setGood(value: Good) {
        if (value != undefined) {
            this.good = value;
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
            this._editGoodForm.valueChanges.subscribe(
                result => {
                    let good = (JSON.parse(JSON.stringify(this.good)));
                    good.fileLink.fileUrl = result.imageUrl;
                    good.price = result.price;
                    good.qty = result.qty;
                    good.name = result.name;
                    this.formValueChanges.emit({valid: this._editGoodForm.valid, good: good})
                }
            );
        }
    }

    @ViewChild('inputFile', { static: false }) imageInputVariable: ElementRef;
    reset() {
        this.imageInputVariable.nativeElement.value = '';
    }

    constructor(@Inject(DataService) private dataService: DataService) {
        this.formValueChanges = new EventEmitter<boolean>();
    }

    uploadFiles(event: any) {
        let files = event.target.files;
        this.dataService.uploadFile(files).subscribe((data: FileLink) => {
            this.good.fileLink = data;
            this.formValueChanges.emit(this._editGoodForm.valid);
        });
    }

    createImgPath = (serverPath: string) => {
        return createImagePath(serverPath);
    } 
}

