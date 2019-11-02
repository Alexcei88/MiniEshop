import { Component, Inject, OnInit, EventEmitter, Output } from '@angular/core';
import { Category } from './../../../model';
import { DataService } from '../../../services/data.service'

@Component({
    selector: 'category',
    templateUrl: './category.component.html',
    styleUrls: ['./../../eshop.component.css']
})

export class CategoryComponent implements OnInit {
    private _category: Category[];  // массив товаров
    private _prevSelectedCategoryId: string

    @Output() categoryWasSelected: EventEmitter<Category>;

    constructor(@Inject(DataService) private dataService: DataService) {
        this.categoryWasSelected = new EventEmitter<Category>(); 
        this._prevSelectedCategoryId = '';
    }

    ngOnInit() {
        this.loadCategories();    // загрузка данных при старте компонента
    }
    // получаем данные через сервис
    loadCategories() {
        this.dataService.getCategories()
            .subscribe((data: Category[]) =>
            {
                this._category = data;
            })
    }

    onCategoryWasSelected(ev: any): void {
        if(this._prevSelectedCategoryId != ev.node.data.id)
        {
            this.categoryWasSelected.emit(ev.node.data);
        }
    }
}

