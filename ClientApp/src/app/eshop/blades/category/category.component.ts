import { Component, Inject, EventEmitter, Output, ViewChild } from '@angular/core';
import { Category } from './../../../model';
import { BladeConfig } from 'ngx-blade/esm5/ngx-blade';
import { NgxBladeComponent } from 'ngx-blade/esm5/src/app/modules/ngx-blade/ngx-blade.component';


@Component({
    selector: 'category-blade',
    templateUrl: './category.component.html',
    styleUrls: ['./../../eshop.component.css']
})

export class CategoryComponent {

    @Output() categoryWasSelected: EventEmitter<Category>;
    private _bladeName: string = "Категории";

    public bladeConfig: BladeConfig = {
        closeButton: false,
        maximizeButton: false,
        isModal: false,
    };

    private _blade: NgxBladeComponent;
    @ViewChild("categoryBlade", { static: false }) set blade(blade: NgxBladeComponent) {
        if (blade !== undefined) {
            this._blade= blade;
            this._blade.onMaximize();
        }
    }

    constructor() {
        this.categoryWasSelected = new EventEmitter<Category>();
    }

    onCategoryWasSelected(category: Category): void {
        this.categoryWasSelected.emit(category);
    }
}

