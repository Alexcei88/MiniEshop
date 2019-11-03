import { Component, Inject, OnInit, EventEmitter, Output } from '@angular/core';
import { Category } from './../../../model';
import { DataService } from '../../../services/data.service'

@Component({
    selector: 'category-body',
    templateUrl: './category-body.component.html',
    styleUrls: ['./../../eshop.component.css']
})

export class CategoryBodyComponent implements OnInit {
    private _category: Category[];

    public get category(): Category[] { return this._category;};
    private _prevSelectedCategoryId: string

    @Output() categoryWasSelected: EventEmitter<Category>;

    constructor(@Inject(DataService) private dataService: DataService) {
        this.categoryWasSelected = new EventEmitter<Category>();
        this._prevSelectedCategoryId = '';
    }

    ngOnInit() {
        this.loadCategories();
    }

    loadCategories() {
        this.dataService.getCategories()
            .subscribe((data: Category[]) => {
                this._category = data;
            })
    }

    onCategoryWasSelected(ev: any): void {
        if(this._prevSelectedCategoryId != ev.node.data.id) {
            this.categoryWasSelected.emit(ev.node.data);
        }
    }
}

