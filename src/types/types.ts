export interface ReportEntryVM {
  comment: string
  duration: number
}

export interface ReportGroupVM {
  headline: string
  entries: ReportEntryVM[]
  total: string
}

export interface ReportVM {
  headline: string
  groups: ReportGroupVM[]
  total: string
}

export interface Activity {
  idx: number
  begin: Date | null
  end: Date | null
  comment: string
}
