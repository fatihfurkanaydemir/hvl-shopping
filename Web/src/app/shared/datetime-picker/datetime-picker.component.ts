import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import {
  NgbTimeStruct,
  NgbDateStruct,
  NgbPopoverConfig,
} from '@ng-bootstrap/ng-bootstrap';

import { DateTime } from './dateTime';

@Component({
  selector: 'app-datetime-picker',
  templateUrl: './datetime-picker.component.html',
  styleUrls: ['./datetime-picker.component.css'],
})
export class DatetimePickerComponent implements OnInit {
  @Input()
  hourStep = 1;
  @Input()
  minuteStep = 15;
  @Input()
  secondStep = 30;

  @Output('OnDatetimeChanged') onDatetimeChangedEvent =
    new EventEmitter<Date>();

  @Input('StartDate')
  startDate: Date | null = null;

  dateString!: string;
  datetime: DateTime = DateTime.fromLocalString(
    this.startDate?.toString() ?? new Date().toString()
  )!;

  constructor(private config: NgbPopoverConfig) {
    config.autoClose = 'outside';
    config.placement = 'auto';
  }

  ngOnInit(): void {
    this.updateDatetime();
    this.setDateStringModel();
  }

  getMinDate(): NgbDateStruct {
    const now = new Date();
    return {
      year: now.getUTCFullYear(),
      day: now.getUTCDate(),
      month: now.getUTCMonth() + 1,
    };
  }

  getMinHour(): NgbTimeStruct {
    const now = new Date();
    return {
      hour: now.getUTCHours() + 1,
      minute: now.getUTCMinutes(),
      second: now.getUTCSeconds(),
    };
  }

  isDateValid() {
    return new Date() <= new Date(this.dateString);
  }

  updateDatetime() {
    const date = this.startDate ? new Date(this.startDate) : new Date();
    date.setUTCHours(date.getUTCHours() + 1);

    this.datetime = DateTime.fromLocalString(date.toString())!;
  }

  onDateChange($event: string | NgbDateStruct) {
    const date = new DateTime($event);

    if (!date) return;

    if (!this.datetime) {
      this.datetime = date;
    }

    this.datetime.year = date.year;
    this.datetime.month = date.month;
    this.datetime.day = date.day;

    const adjustedDate = new Date(this.datetime.toString() ?? '');
    if (this.datetime.timeZoneOffset !== adjustedDate.getTimezoneOffset()) {
      this.datetime.timeZoneOffset = adjustedDate.getTimezoneOffset();
    }

    this.setDateStringModel();

    this.onDatetimeChangedEvent.emit(new Date(this.dateString));
  }

  onTimeChange(event: NgbTimeStruct) {
    this.datetime.hour = event.hour;
    this.datetime.minute = event.minute;
    this.datetime.second = event.second;

    this.setDateStringModel();

    this.onDatetimeChangedEvent.emit(new Date(this.dateString));
  }

  setDateStringModel() {
    this.dateString = this.datetime.toString() ?? '';
  }
}
